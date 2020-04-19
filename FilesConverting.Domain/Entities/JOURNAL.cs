using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilesConverting.Domain.Entities
{
    public class JOURNAL
    {
        public long ID { get; set; }

        public DateTime  UPLOAD{ get; set; }

        public string FILENAME { get; set; }
        
        public byte[] FILECONTENT { get; set; }

        public string FILEMIMETYPE { get; set; }

        public int FILESIZE { get; set; }

    }
}
