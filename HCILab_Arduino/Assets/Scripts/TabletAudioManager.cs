using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabletAudioManager : MonoBehaviour
{
    [SerializeField] private Image[] tabletImages;

    [Header("Audio(BGM, TTS, EffecrSound")] 
    [SerializeField] private AudioSource audiosourceBGM;
    [SerializeField] private AudioClip[] audioClipBGM;
    [Space]
    [SerializeField] private AudioSource audiosourceTTS;
    [SerializeField] private AudioClip[] audioClipTTS;
    [Space]
    [SerializeField] private AudioSource audiosourceEffectSound;
    [SerializeField] private AudioClip[] audioClipEffectSound;

    private void Start()
    {
        ActiveTabletGUI(ImageType.Navigation_normal_sad);
    }

    // Image 활성과 함께 음악, 음성 출력
    public void ActiveTabletGUI(ImageType imageType)
    {
        // 먼저 현재 활성화된 이미지 비활성화 TODO : 추후 코드 수정(이미 비활성화된 오브젝트에 비활성화 할 필요없음)
        DeActiveImages();
        
        // 해당하는 이미지 활성화
        tabletImages[(int)imageType].gameObject.SetActive(true);
        
        // 각 상황에 따른 음성 재생
        switch (imageType)
        {
            case ImageType.Navigation_normal:
                
                break;
            case ImageType.Navigation_normal_anger:
                // TODO : 화, 두려움 감정에 해당하는 음성이 두 가지이다. 이 부분은 합친 후에 코드 수정하는 방향
                audiosourceTTS.clip = audioClipTTS[(int)TTSSound.해피캄_뒷차에위급한환자가있나봐요]; 
                audiosourceTTS.clip = audioClipTTS[(int)TTSSound.해피캄_앞차가바쁜일이있나봐요]; 
                audiosourceTTS.Play();

                StartCoroutine(DelayAndActiveNextTabletGUI(ImageType.Navigation_normal_anger_media));
                break;
            
            case ImageType.Navigation_normal_anger_media:
                audiosourceBGM.clip = audioClipBGM[(int)EmotionMusic.화두려움저감음악];
                audiosourceBGM.Play();
                break;
            
            case ImageType.Navigation_normal_neutral:
                audiosourceTTS.clip = audioClipTTS[(int)TTSSound.해피캄_정상감정입니다안전운전하세요];
                audiosourceTTS.Play();
                break;
            
            case ImageType.Navigation_normal_sad:
                audiosourceTTS.clip = audioClipTTS[(int)TTSSound.해피캄_슬픈감정은운전에방해될수];
                audiosourceTTS.Play();
                
                StartCoroutine(DelayAndActiveNextTabletGUI(ImageType.Navigation_normal_sad_media));
                break;
            
            case ImageType.Navigation_normal_sad_media:
                audiosourceBGM.clip = audioClipBGM[(int)EmotionMusic.슬픔저감음악];
                audiosourceBGM.Play();
                break;
            
            case ImageType.Navigation_normal_stress:
                // TODO : 좌절, 스트레스 감정에 해당하는 음성이 두 가지이다. 이 부분은 합친 후에 코드 수정하는 방향
                audiosourceTTS.clip = audioClipTTS[(int)TTSSound.해피캄_세찬비가오네요ABS장치가]; 
                audiosourceTTS.clip = audioClipTTS[(int)TTSSound.해피캄_앞차가초보운전자인것같아요]; 
                audiosourceBGM.Play();
                
                StartCoroutine(DelayAndActiveNextTabletGUI(ImageType.Navigation_normal_stress_media));
                break;
            
            case ImageType.Navigation_normal_stress_media:
                audiosourceBGM.clip = audioClipBGM[(int)EmotionMusic.좌절스트레스저감음악];
                audiosourceBGM.Play();
                break;
            
            case ImageType.Navigation_normal_surprise:
                // TODO : 놀라는 감정에 해당하는 음성이 두 가지이다. 이 부분은 합친 후에 코드 수정하는 방향
                audiosourceTTS.clip = audioClipTTS[(int)TTSSound.해피캄_도로위장애물로인해놀라셨군요];
                audiosourceTTS.clip = audioClipTTS[(int)TTSSound.해피캄_포트홀로인해놀라셨군요]; 
                audiosourceTTS.Play();
                
                StartCoroutine(DelayAndActiveNextTabletGUI(ImageType.Navigation_normal_surprise_media));
                break;
            
            case ImageType.Navigation_normal_surprise_media:
                audiosourceBGM.clip = audioClipBGM[(int)EmotionMusic.놀람저감음악];
                audiosourceBGM.Play();
                break;
            
            case ImageType.Navigation_normal_warning_구급차추월:
                audiosourceTTS.clip = audioClipTTS[(int)TTSSound.뒷차추월_구급차가후방에서다가오고있습니다];
                audiosourceTTS.Play();
                break;
            
            case ImageType.Navigation_normal_warning_후방차추월:
                audiosourceTTS.clip = audioClipTTS[(int)TTSSound.뒷차추월_뒤에서차가빠르게다가오고있습니다];
                audiosourceTTS.Play();
                break;
            
            case ImageType.Navigation_normal_과속단속구간_단속됨:
                audiosourceTTS.clip = audioClipTTS[(int)TTSSound.과속_직전과속단속구간에서단속되었을확률이높습니다];
                audiosourceTTS.Play();
                break;
            
            case ImageType.Navigation_normal_과속단속구간_정상통과:
                audiosourceTTS.clip = audioClipTTS[(int)TTSSound.과속_직전과속단속구간을정상통과하셨습니다];
                audiosourceTTS.Play();
                break;
            
            case ImageType.Navigation_normal_선바이저알림_강한햇살:
                audiosourceTTS.clip = audioClipTTS[(int)TTSSound.선바이저_전방의강한햇살로선바이저를내립니다];
                audiosourceTTS.Play();
                break;
            
            case ImageType.Navigation_normal_선바이저알림_터널출구:
                audiosourceTTS.clip = audioClipTTS[(int)TTSSound.선바이저_전방에터널출구가있어선바이저를내립니다];
                audiosourceTTS.Play();
                break;
            
            case ImageType.Navigation_normal_우회전가능:
                
                break;
            
            case ImageType.Navigation_normal_우회전불가능:
                break;
        }
    }
    
    // GUI, 오디오 출력을 순차적으로 진행하기 (EX. 슬픈 감정은 운전에 방해됩니다 -> 슬픔 저감 음악)
    // 약간의 대기시간을 주기 위해 코루틴을 사용한다.
    IEnumerator DelayAndActiveNextTabletGUI(ImageType nextTargetImageType,float delayTime = 10) // 파라미터가 없으면 10초 대기 후 바로 넘어감.
    {
        // 일반 주행 상황으로 가야한다면 파라미터 next Target Image Type을 ImageType.Navigation_normal로 설정하면 됨.
        yield return new WaitForSeconds(delayTime); // 5초 대기
        
        ActiveTabletGUI(nextTargetImageType);
    }
    
    
    void DeActiveImages()
    {
        foreach (Image image in tabletImages)
        {
            image.gameObject.SetActive(false);
        }
    }
}

public enum ImageType
{
    Navigation_normal,
    Navigation_normal_anger,
    Navigation_normal_anger_media,
    Navigation_normal_neutral,
    Navigation_normal_sad,
    Navigation_normal_sad_media,
    Navigation_normal_stress,
    Navigation_normal_stress_media,
    Navigation_normal_surprise,
    Navigation_normal_surprise_media,
    Navigation_normal_warning_구급차추월,
    Navigation_normal_warning_후방차추월,
    Navigation_normal_과속단속구간_단속됨,
    Navigation_normal_과속단속구간_정상통과,
    Navigation_normal_선바이저알림_강한햇살,
    Navigation_normal_선바이저알림_터널출구,
    Navigation_normal_우회전가능,
    Navigation_normal_우회전불가능
}

enum EmotionMusic
{
    놀람저감음악,
    슬픔저감음악,
    시끄러운음악,
    좌절스트레스저감음악,
    화두려움저감음악
}

enum TTSSound
{
    과속_전방에과속단속구간입니다제한속도는,
    과속_직전과속단속구간에서단속되었을확률이높습니다,
    과속_직전과속단속구간을정상통과하셨습니다,
    선바이저_전방에터널출구가있어선바이저를내립니다,
    선바이저_전방의강한햇살로선바이저를내립니다,
    뒷차추월_구급차가후방에서다가오고있습니다,
    뒷차추월_뒤에서차가빠르게다가오고있습니다,
    해피캄_뒷차에위급한환자가있나봐요,
    해피캄_앞차가바쁜일이있나봐요,
    해피캄_세찬비가오네요ABS장치가,
    해피캄_앞차가초보운전자인것같아요,
    해피캄_정상감정입니다안전운전하세요,
    해피캄_슬픈감정은운전에방해될수,
    해피캄_도로위장애물로인해놀라셨군요,
    해피캄_포트홀로인해놀라셨군요,
}

enum EffectSound
{
    과속단속정보알림음,
    긍정적인경고음,
    부정적인경고음
}