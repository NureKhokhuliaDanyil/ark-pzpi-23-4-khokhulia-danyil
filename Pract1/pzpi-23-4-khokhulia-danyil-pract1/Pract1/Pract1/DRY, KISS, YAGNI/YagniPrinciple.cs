namespace Pract1;

// --- ПОГАНИЙ ПРИКЛАД ---
// Містить методи "на майбутнє", які зараз не потрібні.
public class BadReportExporter
{
    public void ExportToCsv(string data)
    {
        File.WriteAllText("report.csv", data);
    }

    public void ExportToPdf(string data)
    {
        throw new NotImplementedException("Буде реалізовано пізніше");
    }

    public void ExportToXml(string data)
    {
        throw new NotImplementedException("Буде реалізовано пізніше");
    }
}

// --- ГАРНИЙ ПРИКЛАД ---
// Реалізовано лише те, що вимагається зараз.
public class GoodReportExporter
{
    public void ExportToCsv(string data)
    {
        File.WriteAllText("report.csv", data);
    }
}