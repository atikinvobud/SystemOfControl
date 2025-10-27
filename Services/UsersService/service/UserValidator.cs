using System.Data;
using BackEnd.DTOs;
using BackEnd.Models.Entities;
using FluentValidation;

namespace BackEnd.Services;

public class UserValidator : AbstractValidator<RegistrDTO>
{
    public UserValidator()
    {
        ClassLevelCascadeMode = CascadeMode.Stop;

        RuleFor(dto => dto.Login)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("Поле логина не должно быть пустым")
            .Matches(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$").WithMessage("Почта введена не правильно");

        RuleFor(dto => dto.Password)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("Пароль не должен быть пустым")
            .MinimumLength(8).WithMessage("Пароль должен быть не менее 8 символов");

        RuleFor(dto => dto)
            .Cascade(CascadeMode.Stop)
            .Must(dto => dto.Password == dto.RepeatPassword).WithMessage("Пароли не совпадают");

        RuleFor(dto => dto.Name)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("Имя не должно быть пустым")
            .Length(3, 30).WithMessage("Имя должно быть от 3 до 30 символов")
            .Matches(@"^[А-Я][а-я]+$").WithMessage("Имя должно быть русскими буквами, причем первая заглавная");

        RuleFor(dto => dto.Surname)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("Фамилия не должна быть пустой")
            .Length(5, 30).WithMessage("Фамилия должна быть от 5 до 30 символов")
            .Matches(@"^[А-Я][а-я]+$").WithMessage("Фамилия должна быть русскими буквами, причем первая заглавная");

    }
}