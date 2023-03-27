# WebScvAPI
Создан API сервис по работе с csv файлами.

Работа с сервисом ведется с помощью страницы Swagger

Csv файлы хранятся в папке files

Также сервисе используется бд MongoDb ( эмулируется с помощью Docker)

Иначально база пустая

В ней хранится следующая информация о каждом файле:

Название

Информация о столбцах (их тип: числовой или строковый)

Путь к файлу

Основные методы:

GetAll - получить список с информацией о всех файлах

Add - добавить файл

При добавлении с помощью алгоитма определяется тип каждого столбца, файл заноситя в папку files, а информация о нем в mongodb.

Filtr and Sort - фильтрация и сортировка существующего файла

Delete - удалить информацию о файле из бд и свм файл из папки

Change - заменить существующий файл на новый и обновить информацию о нем в бд.

Также дополнительно:

Добавлена документация Swagger

Реализована контейнеризация приложения

Разработка велась с использованием git flow

Сервис реализован с использованием принципов SOLID и CQRS

Реализованы все CRUD операции

Сервис покрыт Unit тестами

Реализована ауткетификация на основе JWT токенов

Для аутентификации нужно получить токен в методе Authenificate и далее нажать кнопку Authorize и вставить следующую строку:

Без аутентификации доступны только Get методы

Bearer токен

Также для валидации использовались фильтры и библиотека FluentValidation

Чтобы запустить проект нужно сделать следующее:

1. Скачать проект

2. Запустить приложение Docker

3. Выбрать docker-compose в качестве запускаемого проекта

4. Запустить проект

Чтобы запустить тесты нужно:

1. Перейти в репозиторий по ссылке : https://github.com/Eugene13112001/CsvApi.Unittests

2. Скачиваем репозиторий

3.В настройках основного проекта WebSvcApi нажимаем кнопку добавить существующий проект и выбираем Unittests.csproj  из скаченного репозитория

4. В настройках добавленного Unittests нажимаем добавить ссылку на проект и добавляем ссылку на WebSvcApi

5. После этого на верхней панели нажимаем тест -> Запуск всех тестов
