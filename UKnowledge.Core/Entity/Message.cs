using System;
using System.Collections.Generic;
using System.Text;
using CManager.Core.Entity.AuthenticationModels;
using CManager.Core.Entity.Base;

namespace CManager.Core.Entity
{
    public class Message : BaseModel
    {
        public virtual User FromUser { get; set; }
        public int ToRoomId { get; set; }
        public virtual Role ToRole { get; set; }
    }
}
