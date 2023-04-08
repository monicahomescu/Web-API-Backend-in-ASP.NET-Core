using Microsoft.AspNetCore.Mvc;

namespace hwSDI.Controllers
{
    public class Validations : Controller
    {
        public IActionResult ValidateName(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return BadRequest();
            }
            if (name.Length < 3)
            {
                return BadRequest();
            }
            if (!name.Any(char.IsLetter))
            {
                return BadRequest();
            }
            return Ok();
        }
    }
}
