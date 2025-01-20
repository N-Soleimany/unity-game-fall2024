using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camera_movement : MonoBehaviour
{
    public GameObject mainCharacter;

    private Vector3 cameraPosition;
    private Vector3 characterPosition;
    private Vector3 distance;

    public float shakeDuration = 1f; // مدت زمان لرزش دوربین
    public float shakeIntensity = 0.05f; // شدت لرزش دوربین
    private bool isShaking = false;

    private float initialShakeIntensity; // شدت اولیه لرزش (برای ثابت نگه داشتن مقدار)

    void Start()
    {
        cameraPosition = transform.position;
        characterPosition = mainCharacter.transform.position;
        distance = characterPosition - cameraPosition;

        initialShakeIntensity = shakeIntensity; // ذخیره شدت اولیه
    }

    private void LateUpdate()
    {
        characterPosition = mainCharacter.transform.position;

        if (!isShaking)
        {
            cameraPosition = characterPosition - distance;
        }
        else
        {
            float randomOffsetX = Random.Range(-shakeIntensity, shakeIntensity);
            float randomOffsetY = Random.Range(-shakeIntensity, shakeIntensity);
            cameraPosition = characterPosition - distance + new Vector3(randomOffsetX, randomOffsetY, 0);
        }

        transform.position = cameraPosition;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("enemy"))
        {
            StartCoroutine(ShakeCamera());
        }
    }

    private IEnumerator ShakeCamera()
    {
        isShaking = true;
        float elapsedTime = 0f;
        shakeIntensity = initialShakeIntensity; // بازنشانی شدت لرزش به مقدار اولیه

        while (elapsedTime < shakeDuration)
        {
            float progress = elapsedTime / shakeDuration;
            shakeIntensity = Mathf.Lerp(initialShakeIntensity, 0, progress); // کاهش تدریجی شدت لرزش

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        shakeIntensity = initialShakeIntensity; // اطمینان از بازگشت به شدت اولیه
        isShaking = false;
    }
}
