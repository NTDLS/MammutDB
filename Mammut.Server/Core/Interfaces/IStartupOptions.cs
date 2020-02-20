namespace Mammut.Server.Core.Interfaces
{
    public interface IStartupOptions
    {
        public string RootPath { get; set; }
        public string ConfigFile { get; }
    }
}
