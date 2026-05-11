using System;

namespace RBX_Alt_Manager.Classes
{
    public class PrivateServer
    {
        public string Name { get; set; }
        public string JobId { get; set; }
        public DateTime DateAdded { get; set; }

        public PrivateServer()
        {
            DateAdded = DateTime.UtcNow;
        }

        public PrivateServer(string name, string jobId)
        {
            Name = name;
            JobId = jobId;
            DateAdded = DateTime.UtcNow;
        }
    }
}
