using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneFader : MonoBehaviour
{
   public Image Image;
   public float speed=1f;
   public AnimationCurve curve;

   void Start()
   {
      StartCoroutine(FadeIn());
   }

   public void FadeTo(String scene)
   {
      StartCoroutine(FadeOut(scene));   
   }
   
   IEnumerator FadeIn()
   {
      float t = 1f;
      while (t > 0f)
      {
         t -= Time.deltaTime *  speed;
         float alpha = curve.Evaluate(t);
         Image.color =new Color(Image.color.r,Image.color.g,Image.color.b,alpha);
         yield return 0;
      }
   }IEnumerator FadeOut(string scene)
   {
      float t = 0f;
      while (t < 1f)
      {
         t += Time.deltaTime *  speed;
         float alpha = curve.Evaluate(t);
         Image.color =new Color(Image.color.r,Image.color.g,Image.color.b,alpha);
         yield return 0;
      }
      
      SceneManager.LoadScene(scene);
   }
}
