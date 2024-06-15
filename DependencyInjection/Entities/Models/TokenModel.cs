using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class TokenModel
    {
        [JsonPropertyName("AccessToken")]
        public string? AccessToken { get; set; }  
        [JsonPropertyName("RefrehToken")]
        public string? RefreshToken { get; set; }
    }
}
