namespace HWManager.Core.Models
{
    // 같은 이름을 가진 프로세스들을 하나로 집계한 뷰.
    // (예: chrome 8개 인스턴스 → Name=chrome, TotalMemoryMB=합산, InstanceCount=8)
    public class ProcessGroupInfo
    {
        public string Name { get; set; } = string.Empty;
        public double TotalMemoryMB { get; set; }
        public int InstanceCount { get; set; }
    }
}
