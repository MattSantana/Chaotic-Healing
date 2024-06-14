using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Tools;

public class PatientManager : MonoBehaviour
{
    [SerializeField] private GameObject[] patientPrefabs; // Array de prefabs dos pacientes
    [SerializeField] private Transform patientSpawnPoint; // Ponto de spawn do paciente
    [SerializeField] private MMProgressBar patientHealthBar;

    private PotionCrafting potionCrafting;

    private void Awake()
    {
        potionCrafting = FindObjectOfType<PotionCrafting>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            SpawnPatient();
        }
    }

    public void SpawnPatient()
    {
        // Seleciona aleatoriamente um prefab do array
        GameObject selectedPrefab = patientPrefabs[Random.Range(0, patientPrefabs.Length)];

        // Instancia o prefab selecionado no ponto de spawn
        GameObject newPatientObject = Instantiate(selectedPrefab, patientSpawnPoint.position, patientSpawnPoint.rotation);

        // Obtém o componente PatientHealth do objeto instanciado e ativa o paciente
        PatientHealth newPatient = newPatientObject.GetComponent<PatientHealth>();
        if (newPatient != null)
        {
            newPatient.ActivatePatient();
            potionCrafting.ChangeRandomRecipe();
        }

        else
        {
            Debug.LogWarning("O prefab selecionado não possui o componente PatientHealth.");
        }
    }
}
