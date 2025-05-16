using System;
using System.Collections.Generic;


namespace ApiCallLibrary.DTO
{
      public class BulkResponse
    {
        
               
        public int TotalPages { get; set; }        
        public List<User> Data { get; set; }

        
    }

}
