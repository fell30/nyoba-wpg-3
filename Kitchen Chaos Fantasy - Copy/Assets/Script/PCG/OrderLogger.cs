using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

public class OrderLogger : MonoBehaviour
{
    [SerializeField] private string fileName = "pcg_log.csv";

    private const string Header = "timestamp,source,level,generate_id,attempt,is_valid,reason,sequence";
    private bool warned = false;

    public string FilePath
    {
        get { return Path.Combine(Application.persistentDataPath, fileName); }
    }

    public void LogCandidate(LevelConfigSO config, int generateId, int attempt,
                             DependencyCheckResult result, List<KitchenObjectSO> candidate, string source)
    {
        try
        {
            string path = FilePath;
            if (!File.Exists(path))
            {
                File.WriteAllText(path, Header + "\n", Encoding.UTF8);
                Debug.Log("[PCG] Log CSV: " + path);
            }

            string sequence = string.Join("|", candidate.Select(o => o != null ? o.objectName : "NULL").ToArray());

            string line = string.Join(",", new string[]
            {
                System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture),
                source,
                config.levelIndex.ToString(CultureInfo.InvariantCulture),
                generateId.ToString(CultureInfo.InvariantCulture),
                attempt.ToString(CultureInfo.InvariantCulture),
                result.IsValid ? "1" : "0",
                Quote(result.Reason),
                Quote(sequence)
            });

            File.AppendAllText(path, line + "\n", Encoding.UTF8);
            warned = false;
        }
        catch (System.Exception e)
        {
            // Jangan sampai gagal menulis log merusak game. Beri tahu sekali agar tidak membanjiri Console.
            if (!warned)
            {
                warned = true;
                Debug.LogWarning("[PCG] Gagal menulis log CSV, baris TIDAK tercatat (file sedang dibuka di Excel?). "
                    + "Tutup file tersebut lalu ulangi pengambilan data. Detail: " + e.Message);
            }
        }
    }

    [ContextMenu("Hapus file log")]
    public void ClearLog()
    {
        try
        {
            if (File.Exists(FilePath)) File.Delete(FilePath);
            Debug.Log("[PCG] Log dihapus: " + FilePath);
        }
        catch (System.Exception e)
        {
            Debug.LogWarning("[PCG] Gagal menghapus log (file sedang dibuka?): " + e.Message);
        }
    }

    private static string Quote(string value)
    {
        if (value == null) value = "";
        return "\"" + value.Replace("\"", "\"\"") + "\"";
    }
}