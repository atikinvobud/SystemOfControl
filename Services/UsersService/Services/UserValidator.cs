using System.Data;
using BackEnd.DTOs;
using BackEnd.Models.Entities;
using BackEnd.Share;
using FluentValidation;

namespace BackEnd.Services;

public class UserValidator : AbstractValidator<RegistrDTO>
{
    public UserValidator()
    {
        ClassLevelCascadeMode = CascadeMode.Stop;

        RuleFor(dto => dto.Login)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithErrorCode(ErrorCode.EmptyLogin.ToString())
            .Matches(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$").WithErrorCode(ErrorCode.LoginError.ToString());

        RuleFor(dto => dto.Password)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithErrorCode(ErrorCode.EmptyPassword.ToString())
            .MinimumLength(8).WithErrorCode(ErrorCode.PasswordError.ToString());

        RuleFor(dto => dto)
            .Cascade(CascadeMode.Stop)
            .Must(dto => dto.Password == dto.RepeatPassword).WithErrorCode(ErrorCode.PasswordMatch.ToString());

        RuleFor(dto => dto.Name)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithErrorCode(ErrorCode.EmptyName.ToString())
            .Length(2, 30).WithErrorCode(ErrorCode.NameLength.ToString())
            .Matches(@"^[А-Я][а-я]+$").WithErrorCode(ErrorCode.NameError.ToString());

        RuleFor(dto => dto.Surname)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithErrorCode(ErrorCode.EmptySurname.ToString())
            .Length(5, 30).WithErrorCode(ErrorCode.SurnameLength.ToString())
            .Matches(@"^[А-Я][а-я]+$").WithErrorCode(ErrorCode.SurnameError.ToString());

    }
}