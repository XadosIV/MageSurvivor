using UnityEngine;
using System.Diagnostics;
public class PythonRunner : MonoBehaviour
{
    public string RunPythonFileWithResult(string pathToPython, string pathToScript, string arguments = "")
    {
        ProcessStartInfo start = new ProcessStartInfo();
        start.FileName = pathToPython;        // chemin vers python.exe
        start.Arguments = $"\"{pathToScript}\" {arguments}"; // script + arguments
        start.UseShellExecute = false;
        start.RedirectStandardOutput = true;  // récupère le stdout
        start.RedirectStandardError = true;   // récupère le stderr

        using (Process process = Process.Start(start))
        {
            string output = process.StandardOutput.ReadToEnd();
            string error = process.StandardError.ReadToEnd();
            process.WaitForExit();

            if (!string.IsNullOrEmpty(error))
                UnityEngine.Debug.LogError("Python error: " + error);

            return output.Trim(); // retourne le résultat du script
        }
    }
}
