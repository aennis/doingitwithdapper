using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Objects.Dtos
{
  public  class AppServiceDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string LaunchUrl { get; set; }

    }
}
