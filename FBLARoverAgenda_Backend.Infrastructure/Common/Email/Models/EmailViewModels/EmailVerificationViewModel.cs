﻿namespace FBLARoverAgenda_Backend.Infrastructure.Common.Email.Models.EmailViewModels
{
    public class EmailVerificationViewModel : EmailBaseViewModel
    {
        public string Link { get; set; } = string.Empty;
    }
}
