using System;
using System.Collections.Generic;
using System.Text;
using UKnowledge.Core.Entity.AuthenticationModels;
using UKnowledge.Core.Entity.Base;

namespace UKnowledge.Core.Entity
{
    public class Message : BaseModel
    {
        public virtual User FromUser { get; set; }
        public int ToRoomId { get; set; }
        public virtual Role ToRole { get; set; }
    }
}
