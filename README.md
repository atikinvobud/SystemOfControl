Клонировать репозиторий.
```sh
git clone https://github.com/atikinvobud/SystemOfControl
````

Для запуска проекта в среде разработки:

Первый терминал (Пользователи):
````sh
cd Services/UsersService
dotnet run
````
Второй терминал (Заказы):
````sh
cd Services/OrdersService
dotnet run
````
Третий терминал (Шлюз):
````sh
cd Services/ApiGateway
dotnet run
````

Для запуска в среде продакшена:

Запустить docker compose
````sh
docker compose up -d
````

Для тестирования:
````sh
cd Serevices/Tests
dotnet test
````
