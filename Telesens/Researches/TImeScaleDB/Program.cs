using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using TImeScaleDB.Models;

namespace TImeScaleDB;
class MainClass
{
      static void Main(string[] args)
      {
            using (ApplicationContext db = new ApplicationContext())
            {
                  Sensor sensor1 = new Sensor
                  {
                        UTC = new DateTime(2015, 7, 20, 18, 30, 25, DateTimeKind.Utc),
                        GPS = "-89.64591286602015,86.41263478479118",
                        sensor1 = 0.17F,
                        sensor2 = 0.34F,
                        sensor3 = 0.17F,
                        g_sensor = 0.69F,
                        volt = 12.27F,
                        temp = 271,
                        hum = 18,
                        barometer = 1030.36F
                  };

                  AlertRule alertRule1 = new AlertRule
                  {
                        ID = 1456,
                        Type = AlertType.Information,
                        CriticalValue = 10.252F,
                        Operator = Operator.LessThanOrEqual,
                        ValueName = "GPS"
                  };

                  NotificationGroup group1 = new NotificationGroup
                  {
                        ID = 1144,
                        Name = "SisunTransportation"
                  };

                  TelegramChat telegram1 = new TelegramChat
                  {
                        ID = 14645,
                        TelegramChatId = "-4244865051",
                        NotificationGroupId = group1.ID,
                        NotificationGroup = group1
                  };

                  TelegramChat telegram2 = new TelegramChat
                  {
                        ID = 7432,
                        TelegramChatId = "-1412524536",
                        NotificationGroupId = group1.ID,
                        NotificationGroup = db.Groups.FirstOrDefault(g => g.ID==group1.ID)
                  };

                  TelegramChat telegram3 = new TelegramChat
                  {
                        ID = 962,
                        TelegramChatId = "-2075214531",
                        NotificationGroupId = group1.ID,
                        NotificationGroup = db.Groups.FirstOrDefault(g => g.ID==group1.ID)
                  };

                  AlertRuleNotificationGroup groupAlert1 = new AlertRuleNotificationGroup
                  {
                        ID = 1234,
                        AlertRuleId = alertRule1.ID,
                        AlertRule = alertRule1,
                        NotificationGroupId = group1.ID,
                        NotificationGroup = group1
                  };

                  db.Sensors.AddIfNotExists(sensor1, s => s.UTC == sensor1.UTC);
                  db.Rules.AddIfNotExists(alertRule1, r => r.ID == alertRule1.ID);
                  db.Groups.AddIfNotExists(group1, g => g.ID == group1.ID);
                  db.Telegrams.AddIfNotExists(telegram1, t => t.ID == telegram1.ID);
                  db.Telegrams.AddIfNotExists(telegram2, t => t.ID == telegram2.ID);
                  db.Telegrams.AddIfNotExists(telegram3, t => t.ID == telegram3.ID);
                  db.GroupAlerts.AddIfNotExists(groupAlert1, ga => ga.ID == groupAlert1.ID);
                  db.SaveChanges();

                  var sensors = db.Sensors.ToList();
                  Console.WriteLine("Содержимое таблицы:");
                  foreach (Sensor s in sensors)
                  {
                        Console.WriteLine($"UTC: {s.UTC} GPS: {s.GPS}");
                  }

                  var rules = db.Rules.ToList();
                  foreach (AlertRule r in rules)
                  {
                        var operString = Operator.GetName(typeof(Operator), r.Operator);
                        var typeString = AlertType.GetName(typeof(AlertType), r.Type);
                        Console.WriteLine($"{typeString}: {r.ValueName} value is {operString} {r.CriticalValue}");
                  }
            }
      }
}