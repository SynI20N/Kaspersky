<h1>Имитация Банковского автомата</h1>
<h2>Предварительные требования: ASP.NET Core 8.0</h2>
<p>Компиляция: <code>dotnet build</code></p>
<p>Запуск: <code>dotnet run</code></p>
<p>Запуск с Docker: <code>sudo docker compose up -d</code></p>

<p>Как различные выборы влияют на алгоритм:</p>
<table>
  <tr>
    <td><img src="https://github.com/SynI20N/Cashpoint/blob/main/img/1.png" height="400" width="400"></td>
    <td><img src="https://github.com/SynI20N/Cashpoint/blob/main/img/2.png" height="400" width="400"></td>
  </tr>
  <tr>
    <td><img src="https://github.com/SynI20N/Cashpoint/blob/main/img/3.png" height="400" width="400"></td>
    <td><img src="https://github.com/SynI20N/Cashpoint/blob/main/img/4.png" height="400" width="400"></td>
  </tr>
</table>

<p>Трудный случай с простыми числами:</p>
<table>
  <tr>
    <td><img src="https://github.com/SynI20N/Cashpoint/blob/main/img/5.png" height="400" width="400"></td>
  </tr>
</table>
<!-- <h2>DDOS attack protection</h2>
<p>Leaky bucket is used to protect from DDOS as a middleware</p>
<p>For the following reasons:
  <ul>
  <li>Easy to implement</li>
  <li>Faster than other algorithms</li>
  </ul> -->
  <h3>Защита от DoS с использованием LeakyBucketMiddleware</h3>

#### Параметры лимитёра

```csharp
public class LeakyBucketOptions
{
    public int BucketSize { get; set; } = 100; // Размер ведра
    public int ProcessingRate { get; set; } = 1; // Скорость обработки запросов в секунду
    public int Timeout { get; set; } = 1000; // Время между проверками в миллисекундах
}
```

#### Механизм работы

1. **Обработка входящих запросов:**
   - Получаем IP-адрес клиента
   - Проверяем наличие данного IP в словаре
   - Пытаемся добавить запрос в ведро
   - Если ведро полное - возвращаем 429 Too Many Requests

2. **Опорожнение ведер:**
   - Таймер срабатывает каждые `_options.Timeout` миллисекунд
   - Для каждого IP уменьшаем количество токенов
   - Скорость опустошения определяется `_options.ProcessingRate`

#### Интеграция в приложение

```csharp
public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.Configure<LeakyBucketOptions>(options =>
        {
            options.BucketSize = 15;
            options.ProcessingRate = 10;
            options.Timeout = 1000;
        });

        builder.Services.AddControllersWithViews();

        var app = builder.Build();

        // ...

        app.UseLeakyBucket();

        // ...
    }
}
```

#### Преимущества решения

1. **Защита от DDoS-атак:**
   - Ограничение количества запросов от одного IP
   - Плавное регулирование нагрузки

2. **Гибкая конфигурация:**
   - Настройка размера ведра
   - Регулировка скорости обработки
   - Настройка интервала проверок

3. **Параллельная обработка:**
   - Использование ConcurrentDictionary
   - Атомарные операции с токенами

4. **Простота реализации и интеграции**

#### Рекомендации по настройке параметров
   - `BucketSize` должен соответствовать ожидаемой нагрузке
   - `ProcessingRate` определяет допустимую скорость запросов
   - `Timeout` влияет на точность ограничения
</p>

#### Нагрузочное тестирование

<p>Контейнер <strong>Artillery-image</strong> создается на основе <code>Dockerfile_test</code>.</p>
<p>Этот контейнер запускает инструмент <strong>Artillery</strong> для выполнения нагрузочного тестирования на определенном порту, предназначенном для Cashpoint.</p>
<p>В файле <code>test-config.yml</code> находятся параметры запуска</p>

```yaml
config:
  target: 'http://host.docker.internal:5282' # локальный адрес
  phases:
    - duration: 60  # 1 минута
      arrivalRate: 2000  # 3 VUs with 1000 requests per VU
      maxVirtualUsers: 3  # Limit the number of VUs to 3

scenarios:
  - flow:
      - get:
          url: "/"
```

<p>Файл <code>result.json</code> содержит результаты нагрузочного тестирования, включая информацию о производительности, количестве запросов, времени отклика и других метриках, которые помогают оценить устойчивость и эффективность системы при высокой нагрузке.</p>
