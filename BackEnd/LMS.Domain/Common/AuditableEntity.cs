using System;
using System.Collections.Generic;
using System.Text;

namespace LMS.Domain.Common
{
    public class AudittableEntity:BaseEntity
    {
        public Guid? LastUpdatedBy { get; set; }
        public Guid? CreatedBy { get; set; }
    }
}