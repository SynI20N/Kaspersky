# TrackSense.ThirdParty.TimescaleDB

<h1>Образ общей базы данных на основе EFCore</h1>
<p>Запуск: <code>docker compose up -d</code></p>
<p>При этом:</p>
<ul>
  <li>Поднимется контейнер с postgres-timescale</li>
  <li>Создастся виртуальная сеть timescaledb_network для подключения сервисов к бд в докере</li>
  <li>Применяться все миграции, если они ещё не были применены</li>
  <li>Добавятся тестовые данные для телеграм бота</li>
</ul>
<p>Остановка: <code>docker compose down</code></p>
