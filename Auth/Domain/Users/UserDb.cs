using System;
using Auth.Domain.Shared;
using Newtonsoft.Json;

namespace Auth.Domain.Users
{
    //cria o Id para a Base de Dados
    public class UserDb : EntityId
    {

        [JsonConstructor]
        public UserDb(Guid value) : base(value)
        { }

        public UserDb(String value) : base(value)
        { }
        override
           protected Object createFromString(String text)
        {
            return new Guid(text);
        }

        override
        public String AsString()
        {
            Guid obj = (Guid)base.ObjValue;
            return obj.ToString();
        }


        public Guid AsGuid()
        {
            return (Guid)base.ObjValue;
        }

    }
}