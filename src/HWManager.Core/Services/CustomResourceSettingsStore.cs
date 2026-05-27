using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using HWManager.Core.Models;

namespace HWManager.Core.Services
{
    /// <summary>
    /// 커스텀 자원 관리 설정을 활성 설정과 개인별 프로필로 저장/불러오는 저장소.
    /// </summary>
    public sealed class CustomResourceSettingsStore
    {
        // 활성 설정: 사용자가 화면에서 바꾼 현재 상태를 자동 저장하는 파일.
        private const string ActiveSettingsFileName = "custom_resource_settings.json";

        // 기존 버전에서 Client가 직접 저장하던 파일명. 최초 전환 시 기존 사용자 설정을 잃지 않기 위해 읽기만 지원한다.
        private const string LegacySettingsFileName = "focus_mode_settings.json";

        // 개인별 프리셋은 활성 설정과 분리해서 여러 개를 보관한다.
        private const string ProfileDirectoryName = "custom_resource_profiles";

        private static readonly JsonSerializerOptions JsonOptions = new JsonSerializerOptions { WriteIndented = true };

        private readonly string _activeSettingsPath;
        private readonly string _legacySettingsPath;
        private readonly string _profileDirectory;

        // 테스트나 다른 호스트에서 저장 위치를 바꿀 수 있게 baseDirectory를 받는다.
        // 실제 앱에서는 AppContext.BaseDirectory 아래에 설정 파일을 둔다.
        public CustomResourceSettingsStore(string? baseDirectory = null)
        {
            string root = string.IsNullOrWhiteSpace(baseDirectory) ? AppContext.BaseDirectory : baseDirectory;
            _activeSettingsPath = Path.Combine(root, ActiveSettingsFileName);
            _legacySettingsPath = Path.Combine(root, LegacySettingsFileName);
            _profileDirectory = Path.Combine(root, ProfileDirectoryName);
        }

        // 화면을 열 때 자동으로 복원할 현재 설정을 읽는다.
        // 새 파일이 없으면 구버전 파일을 읽어 자연스럽게 마이그레이션한다.
        public CustomResourceSettings LoadActiveSettings()
        {
            if (File.Exists(_activeSettingsPath))
                return ReadSettings(_activeSettingsPath);

            if (File.Exists(_legacySettingsPath))
                return ReadSettings(_legacySettingsPath);

            return new CustomResourceSettings();
        }

        // 사용자가 임계값/목록/활성화 상태를 바꿀 때 즉시 저장되는 현재 설정.
        public void SaveActiveSettings(CustomResourceSettings settings)
        {
            WriteSettings(_activeSettingsPath, NormalizeSettings(settings));
        }

        // UI 콤보박스에 표시할 개인 설정 이름 목록을 반환한다.
        // 파일명보다 JSON 내부 ProfileName을 우선해서 사용하므로 파일을 다른 PC에서 가져와도 이름이 유지된다.
        public IReadOnlyList<string> GetProfileNames()
        {
            if (!Directory.Exists(_profileDirectory))
                return Array.Empty<string>();

            return Directory.EnumerateFiles(_profileDirectory, "*.json")
                .Select(ReadProfileName)
                .Where(name => !string.IsNullOrWhiteSpace(name))
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .OrderBy(name => name, StringComparer.OrdinalIgnoreCase)
                .ToList();
        }

        // 선택한 개인 설정을 로드한다. ProfileName이 없는 오래된/수동 작성 파일은 파일명으로 보정한다.
        public CustomResourceSettings LoadProfile(string profileName)
        {
            string normalizedName = NormalizeProfileName(profileName);
            string path = GetProfilePath(normalizedName);
            if (!File.Exists(path))
                throw new FileNotFoundException("저장된 개인 설정을 찾을 수 없습니다.", path);

            CustomResourceSettings settings = ReadSettings(path);
            if (string.IsNullOrWhiteSpace(settings.ProfileName))
                settings.ProfileName = normalizedName;
            return settings;
        }

        // 현재 설정을 사용자가 지정한 이름의 개인 설정 파일로 저장한다.
        // 반환값은 파일명으로 안전하게 보정된 실제 저장 이름이다.
        public string SaveProfile(string profileName, CustomResourceSettings settings)
        {
            string normalizedName = NormalizeProfileName(profileName);
            WriteSettings(GetProfilePath(normalizedName), NormalizeSettings(settings, normalizedName));
            return normalizedName;
        }

        // 필요 없어진 개인 설정 파일을 삭제한다. 현재 활성 설정 파일은 건드리지 않는다.
        public void DeleteProfile(string profileName)
        {
            string normalizedName = NormalizeProfileName(profileName);
            string path = GetProfilePath(normalizedName);
            if (!File.Exists(path))
                throw new FileNotFoundException("삭제할 개인 설정을 찾을 수 없습니다.", path);

            File.Delete(path);
        }

        // 다른 PC로 옮길 수 있도록 현재 설정을 사용자가 고른 위치의 JSON 파일로 내보낸다.
        public void ExportSettings(CustomResourceSettings settings, string destinationPath, string? profileName = null)
        {
            if (string.IsNullOrWhiteSpace(destinationPath))
                throw new ArgumentException("내보낼 파일 경로가 올바르지 않습니다.", nameof(destinationPath));

            string? normalizedName = string.IsNullOrWhiteSpace(profileName) ? null : NormalizeProfileName(profileName);
            WriteSettings(destinationPath, NormalizeSettings(settings, normalizedName));
        }

        // 저장된 개인 설정 하나를 외부 파일로 내보낼 때 사용하는 API.
        public void ExportProfile(string profileName, string destinationPath)
        {
            if (string.IsNullOrWhiteSpace(destinationPath))
                throw new ArgumentException("내보낼 파일 경로가 올바르지 않습니다.", nameof(destinationPath));

            CustomResourceSettings settings = LoadProfile(profileName);
            WriteSettings(destinationPath, NormalizeSettings(settings, settings.ProfileName));
        }

        // 다른 PC에서 가져온 JSON 파일을 개인 설정 폴더로 복사/정규화한다.
        // 이름 인자가 없으면 JSON의 ProfileName, 그것도 없으면 파일명을 설정 이름으로 사용한다.
        public string ImportProfile(string sourcePath, string? profileName = null)
        {
            if (string.IsNullOrWhiteSpace(sourcePath) || !File.Exists(sourcePath))
                throw new FileNotFoundException("가져올 설정 파일을 찾을 수 없습니다.", sourcePath);

            CustomResourceSettings settings = ReadSettings(sourcePath);
            string name = string.IsNullOrWhiteSpace(profileName) ? settings.ProfileName : profileName;
            if (string.IsNullOrWhiteSpace(name))
                name = Path.GetFileNameWithoutExtension(sourcePath);

            return SaveProfile(name, settings);
        }

        // 목록 표시용 이름만 읽는 과정에서 파일 하나가 깨져도 전체 목록 로드를 막지 않기 위한 헬퍼.
        private string ReadProfileName(string path)
        {
            try
            {
                CustomResourceSettings settings = ReadSettings(path);
                return string.IsNullOrWhiteSpace(settings.ProfileName)
                    ? Path.GetFileNameWithoutExtension(path)
                    : settings.ProfileName;
            }
            catch
            {
                return Path.GetFileNameWithoutExtension(path);
            }
        }

        private string GetProfilePath(string profileName)
        {
            Directory.CreateDirectory(_profileDirectory);
            return Path.Combine(_profileDirectory, profileName + ".json");
        }

        private static CustomResourceSettings ReadSettings(string path)
        {
            string json = File.ReadAllText(path);
            return NormalizeSettings(JsonSerializer.Deserialize<CustomResourceSettings>(json) ?? new CustomResourceSettings());
        }

        private static void WriteSettings(string path, CustomResourceSettings settings)
        {
            string? directory = Path.GetDirectoryName(path);
            if (!string.IsNullOrEmpty(directory))
                Directory.CreateDirectory(directory);

            File.WriteAllText(path, JsonSerializer.Serialize(settings, JsonOptions));
        }

        // 파일에서 읽었거나 외부에서 가져온 설정을 앱이 기대하는 형태로 정리한다.
        // 이 단계에서 임계값 범위, 프로세스 이름 표기, 중복 항목을 한 번에 정리한다.
        private static CustomResourceSettings NormalizeSettings(CustomResourceSettings settings, string? profileName = null)
        {
            return new CustomResourceSettings
            {
                ProfileName = profileName ?? settings.ProfileName ?? string.Empty,
                CpuThreshold = ClampThreshold(settings.CpuThreshold),
                RamThreshold = ClampThreshold(settings.RamThreshold),
                GpuThreshold = ClampThreshold(settings.GpuThreshold),
                Enabled = settings.Enabled,
                AutoKillTargets = NormalizeProcessNames(settings.AutoKillTargets),
                TriggerPrograms = NormalizeProcessNames(settings.TriggerPrograms)
            };
        }

        private static List<string> NormalizeProcessNames(IEnumerable<string>? names)
        {
            return (names ?? Array.Empty<string>())
                .Select(ProcessService.NormalizeName)
                .Where(name => !string.IsNullOrWhiteSpace(name))
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();
        }

        private static float ClampThreshold(float value)
        {
            if (float.IsNaN(value)) return 80f;
            return Math.Clamp(value, 10f, 100f);
        }

        // 설정 이름은 그대로 파일명이 되므로 Windows 파일명으로 사용할 수 없는 문자는 '_'로 치환한다.
        private static string NormalizeProfileName(string profileName)
        {
            string name = (profileName ?? string.Empty).Trim();
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("설정 이름을 입력해 주세요.", nameof(profileName));

            foreach (char invalid in Path.GetInvalidFileNameChars())
                name = name.Replace(invalid, '_');

            name = name.Trim().Trim('.');
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("설정 이름을 입력해 주세요.", nameof(profileName));

            return name;
        }
    }
}
