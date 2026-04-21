using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;
using FinanceME.Models.Entities;

namespace FinanceME.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterConfirmationModel : PageModel
    {
        private readonly UserManager<User> _userManager;

        public RegisterConfirmationModel(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        /// <summary>Email address to display.</summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>Whether to show the manual confirmation link (dev-mode fallback).</summary>
        public bool DisplayConfirmAccountLink { get; set; }

        /// <summary>The confirmation URL shown when no real email sender is configured.</summary>
        public string? EmailConfirmationUrl { get; set; }

        public async Task<IActionResult> OnGetAsync(string? email, string? returnUrl = null)
        {
            if (email == null)
            {
                return RedirectToPage("/Index");
            }

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return NotFound($"Unable to load user with email '{email}'.");
            }

            Email = email;

            // Only show the dev-mode manual link when the account is NOT already confirmed
            // and there is no real email sender wired up (IEmailSender is the no-op default).
            DisplayConfirmAccountLink = !await _userManager.IsEmailConfirmedAsync(user);

            if (DisplayConfirmAccountLink)
            {
                var userId = await _userManager.GetUserIdAsync(user);
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

                EmailConfirmationUrl = Url.Page(
                    "/Account/ConfirmEmail",
                    pageHandler: null,
                    values: new { area = "Identity", userId, code, returnUrl },
                    protocol: Request.Scheme);
            }

            return Page();
        }
    }
}
