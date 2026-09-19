# Sales Analyzer

CLI-приложение на **.NET 10** для анализа данных о продажах из CSV-файла

Приложение читает данные о продажах, выполняет набор аналитических операций и выводит результат в консоль либо сохраняет его в JSON-файл

Проект демонстрирует работу с:

- CSV и JSON;
- LINQ;
- `async` / `await`;
- PLINQ;
- Dependency Injection;
- паттерном Strategy;
- Builder;
- обработкой ошибок;
- unit-тестированием с xUnit и Moq.

---

## Возможности

Приложение рассчитывает следующие показатели:

1. общую сумму продаж по категориям;
2. топ-4 категорий по количеству проданных товаров;
3. среднюю цену по месяцам;
4. топ-5 покупателей по рейтингу;
5. среднее время доставки;
6. среднюю скидку по категориям и месяцам.

Также можно ограничить анализ временным диапазоном с помощью аргументов `--start_date` и `--end_date`

`start_date` включается в диапазон, `end_date` — не включается

---

## Требования

Для запуска необходим:

```text
.NET 10 SDK
```

Проверить установленную версию:

```bash
dotnet --version
```

---

## Сборка

Из корневой директории проекта:

```bash
dotnet restore
dotnet build
```

Release-сборка:

```bash
dotnet build -c Release
```

---

## Запуск

Общий формат команды:

```bash
dotnet run --project CLI -- --mode=<mode> --input=<path> [options]
```

### Console

Синхронное чтение CSV, последовательное вычисление аналитики и вывод результата в консоль

```bash
dotnet run --project CLI -- --mode=console --input=sales_5000.csv
```

### File

Синхронная обработка с сохранением результата в JSON-файл

```bash
dotnet run --project CLI -- \
  --mode=file \
  --input=sales_5000.csv \
  --output=result.json
```

### Async

Асинхронное чтение входного файла и асинхронный вывод результата

```bash
dotnet run --project CLI -- --mode=async --input=sales_5000.csv
```

### Parallel

Чтение данных выполняется обычным способом, а аналитические вычисления — параллельно с использованием PLINQ

```bash
dotnet run --project CLI -- --mode=parallel --input=sales_5000.csv
```

### DI

Компоненты приложения создаются через `Microsoft.Extensions.DependencyInjection`

```bash
dotnet run --project CLI -- --mode=di --input=sales_5000.csv
```

### Full

Комбинированный режим:

- асинхронный ввод;
- параллельные вычисления;
- асинхронный вывод;
- Dependency Injection.

```bash
dotnet run --project CLI -- \
  --mode=full \
  --input=sales_5000.csv \
  --output=result.json
```

---

## Аргументы командной строки

| Аргумент | Обязательный | Описание |
|---|---:|---|
| `--mode`, `-m` | Да | Режим работы приложения |
| `--input` | Да | Путь к входному CSV-файлу |
| `--output` | Зависит от режима | Путь к выходному JSON-файлу |
| `--start_date` | Нет | Начальная дата анализируемого периода |
| `--end_date` | Нет | Конечная дата периода, не включительно |
| `--help` | Нет | Вывод справки |

Поддерживаемые режимы:

```text
console
file
async
parallel
di
full
```

Пример анализа ограниченного периода:

```bash
dotnet run --project CLI -- \
  --mode=console \
  --input=sales_5000.csv \
  --start_date=2022-01-01 \
  --end_date=2022-03-01
```

---

## Формат входного файла

CSV должен содержать заголовок и данные следующего формата:

```csv
order_id,order_date,customer_id,product_category,region,quantity,unit_price,discount,payment_method,delivery_days,customer_rating,revenue
10001,1/1/2022,1102,Beauty,South,7,373.65,0.28,Wallet,10,4.7,1883.2
10002,1/2/2022,1435,Clothing,South,7,47.74,0.09,Card,6,3.9,304.1
```

Заголовок файла при чтении пропускается

Каждая строка преобразуется в объект `Sale`

---

## Архитектура

Проект разделён на ответственность между Core-логикой и CLI-слоем

```text
SalesAnalyzer
│
├── Core
│   ├── Model
│   │   ├── Sale
│   │   └── AnalyticsResult
│   │
│   └── Service
│       ├── Parser
│       ├── Reader
│       ├── Analytics
│       └── Writer
│
├── CLI
│   ├── CommandLine
│   ├── ApplicationRunner
│   │   ├── Strategy
│   │   │   ├── Input
│   │   │   ├── Analytics
│   │   │   └── Output
│   │   └── ApplicationRunnerBuilder
│   │
│   └── DependencyInjection
│
└── Tests
```

### Core

`Core` содержит бизнес-логику приложения и не зависит от конкретного способа запуска программы

Основные абстракции:

```text
ICsvReader
ISaleParser
IAnalyticsService
IResultWriter
```

Это позволяет независимо заменять способы чтения, вычисления и вывода данных

---

## ApplicationRunner

За выполнение полного сценария обработки отвечает `ApplicationRunner`

Типичный pipeline выглядит следующим образом:

```text
Input
  ↓
CSV parsing
  ↓
IReadOnlyCollection<Sale>
  ↓
Analytics
  ↓
AnalyticsResult
  ↓
Output
```

---

## ApplicationRunnerBuilder

Для конфигурации `ApplicationRunner` используется Builder

В зависимости от выбранного режима Builder получает необходимые стратегии и собирает готовый runner

Условно:

```csharp
builder
    .WithInputStrategy(...)
    .WithAnalyticsStrategy(...)
    .WithOutputStrategy(...)
    .Build();
```

Builder также гарантирует, что перед запуском настроены все необходимые части pipeline

---

## Чтение CSV

Чтение данных вынесено в отдельную абстракцию:

```csharp
ICsvReader
```

Reader отвечает только за:

- последовательное чтение строк;
- пропуск CSV-заголовка;
- пропуск пустых строк;
- передачу строки в `ISaleParser`.

Для синхронного чтения используется `TextReader`

Асинхронная версия использует:

```csharp
ReadLineAsync()
```

Использование `TextReader` вместо непосредственной зависимости от файла позволяет тестировать reader через:

```csharp
StringReader
```

без обращения к реальной файловой системе.

---

## Аналитика

Последовательная реализация аналитики использует LINQ:

```text
Where
GroupBy
Sum
Average
OrderBy
Take
```

Перед вычислением аналитики к исходной последовательности применяется фильтрация по датам.

Концептуально:

```csharp
var query = sales.AsEnumerable();

if (startDate is not null)
{
    query = query.Where(x => x.OrderDate >= startDate);
}

if (endDate is not null)
{
    query = query.Where(x => x.OrderDate < endDate);
}
```

Такой подход позволяет независимо задавать:

- только `start_date`;
- только `end_date`;
- обе границы;
- отсутствие временного ограничения.

---

## Параллельные вычисления

Для режима `parallel` используется PLINQ.

```csharp
sales.AsParallel()
```

---

## Асинхронность

Асинхронность используется прежде всего для I/O-bound операций:

```text
чтение CSV
запись результата
```

Асинхронная версия чтения использует `ReadLineAsync`

Для вычислений отдельные потоки ввода-вывода не требуются: аналитика является CPU-bound операцией и выполняется отдельно от асинхронного файлового I/O

---

## Dependency Injection

В режимах `di` и `full` зависимости регистрируются через:

```text
Microsoft.Extensions.DependencyInjection
```

DI используется для связывания:

- CSV reader;
- parser;
- analytics services;
- writers;
- strategies;
- `ApplicationRunner`.

Бизнес-логика зависит от интерфейсов, а конкретные реализации выбираются на уровне конфигурации приложения.

---

## Вывод результата

Поддерживаются два основных варианта вывода.

### Console output

Результат преобразуется в человекочитаемый текст и выводится в стандартный поток:

```text
========================================
Sales Analytics
========================================
...
```

### JSON output

Результат сериализуется и сохраняется в JSON-файл.

Для сериализации используется JSON writer приложения.

---

## Обработка ошибок

В точке входа приложения используется глобальная обработка исключений

Обрабатываются, в частности:

- входной файл не существует;
- нет прав на чтение файла;
- входной путь недоступен;
- директория для выходного файла не существует;
- нет прав на запись;
- невозможно создать выходной файл;
- CSV имеет некорректный формат;
- значение поля невозможно преобразовать к ожидаемому типу;
- переданы некорректные аргументы командной строки.

При ошибке приложение завершается с ненулевым exit code.

---

## Тесты

Для тестирования используются:

```text
xUnit
Moq
```

Запустить все тесты:

```bash
dotnet test
```

Release:

```bash
dotnet test -c Release
```

Тестами покрываются основные компоненты приложения:

- CSV parser;
- CSV reader;
- аналитические вычисления;
- async analytics;
- parallel analytics;
- input/output strategies;
- `ApplicationRunner`;
- `ApplicationRunnerBuilder`;
- command-line parser;
- DI configuration.

Для тестов файловой логики используются in-memory реализации:

```csharp
StringReader
StringWriter
```

за счёт чего тесты не зависят от состояния файловой системы.

---

## Публикация

Создать Release-версию приложения:

```bash
dotnet publish ./CLI/CLI.csproj \
  -c Release \
  -o ./publish \
  --self-contained false
```

После публикации приложение можно запустить из директории `publish`

Например:

```bash
dotnet ./publish/CLI.dll \
  --mode=console \
  --input=sales_5000.csv
```

---

## Основные архитектурные решения

В проекте сделан упор на разделение ответственности.

**Strategy Pattern** используется для переключения между синхронным, асинхронным и параллельным поведением без условной логики внутри `ApplicationRunner`

**Builder Pattern** используется для конфигурации и создания `ApplicationRunner`

**Dependency Injection** позволяет отделить создание объектов от бизнес-логики и использовать абстракции вместо прямых зависимостей от реализаций

**TextReader / TextWriter** используются вместо прямой работы с файлами на уровне Core-сервисов, что упрощает тестирование

**LINQ / PLINQ** позволяют использовать одну модель аналитических вычислений как для последовательного, так и для параллельного режима

**Global exception handling** централизует преобразование технических ошибок в понятные пользователю сообщения

---

## Технологии

- .NET 10
- C#
- LINQ
- PLINQ
- async / await
- Microsoft.Extensions.DependencyInjection
- System.Text.Json
- xUnit
- Moq

---

## Запуск тестового сценария

```bash
dotnet restore
dotnet build
dotnet test

dotnet run --project CLI -- \
  --mode=full \
  --input=sales_5000.csv \
  --output=result.json
```

После выполнения результат аналитики будет записан в:

```text
result.json
```