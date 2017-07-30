//using UnityEngine;
//using System.Collections;
//using Slate;

//public class CutsceneManager : Singleton<CutsceneManager>
//{
//    public static bool _isPlaying;

//    [SerializeField]
//    private Cutscene _baseGID1;
//    [SerializeField]
//    private Cutscene _baseGID2;
//    [SerializeField]
//    private Cutscene _baseGIDEnd;
//    [SerializeField]
//    private Cutscene _utilCutscene;

//    private Cutscene _currentCutscene;
//    public bool baseGid1SectionFound;
//    private bool isSkipped = false;
//    private string nextChapterForPlay;
//    private string currentSectionName;
//    private string[] allSections;
//    private string[] _baseGid1Sections;
//    private string[] _baseGid2Sections;
//    private string[] _baseGidEndSections;
//    //public static bool canPlay = true;
//    private TableElement[] test;
//    public bool isStop;

//    void Start()
//    {
//        Cutscene.OnCutsceneStopped += Cutscene_OnCutsceneStopped;
//        _baseGid1Sections = _baseGID1.GetSectionNames();
//        _baseGid2Sections = _baseGID2.GetSectionNames();
//        _baseGidEndSections = _baseGIDEnd.GetSectionNames();

//        allSections = new string[_baseGid1Sections.Length + _baseGid2Sections.Length + _baseGidEndSections.Length];

//        for (int i = 0; i < _baseGid1Sections.Length; i++)
//        {
//            allSections[i] = _baseGid1Sections[i];
//        }

//        for (int i = 0; i < _baseGid2Sections.Length; i++)
//        {
//            allSections[i + _baseGid1Sections.Length] = _baseGid2Sections[i];
//        }

//        for (int i = 0; i < _baseGidEndSections.Length; i++)
//        {
//            //	allSections [i] = _baseGidEndSections [i];
//            allSections[i + _baseGid2Sections.Length + _baseGid1Sections.Length] = _baseGid2Sections[i];
//        }

//        test = GameObject.FindObjectsOfType<TableElement>();
//    }

//    private void Cutscene_OnCutsceneStopped(Cutscene obj)
//    {
//        _isPlaying = false;

//        if (obj.name == "BaseGIDCutsceneEnd")
//        {
//            _baseGIDEnd.RewindNoUndo();
//            _baseGID2.RewindNoUndo();
//            _baseGID1.RewindNoUndo();
//        }
//    }

//    public void StopCutscene()
//    {
//        isStop = true;

//        if (_currentCutscene != null)
//        {
//            _currentCutscene.Stop();
//            return;
//        }

//        if (_baseGID1 != null && _baseGID1.isActive)
//        {
//            _baseGID1.Stop(Cutscene.StopMode.Skip);
//            return;
//        }

//        if (_baseGID2 != null && _baseGID2.isActive)
//        {
//            _baseGID2.Stop(Cutscene.StopMode.Rewind);
//            return;
//        }

//        if (_baseGIDEnd != null && _baseGIDEnd.isActive)
//        {
//            _baseGIDEnd.Stop(Cutscene.StopMode.Skip);
//            return;
//        }
//    }

//    public void PlayBaseGIDCutscene1()
//    {
//        isSkipped = false;
//        _currentCutscene = _baseGID1;
//        _baseGID1.Play(() => { Debug.Log("!PlayBaseGIDCutscene1"); Test(); });
//        //DeactivateButton();
//    }

//    public void PlayBaseGIDCutscene2(string sectionName = "")
//    {
//        _isPlaying = true;
//        _baseGID2.Play();
//        //_baseGID2.PlaySection(sectionName);
//        if (!string.IsNullOrEmpty(sectionName))
//        {
//            _baseGID2.JumpToSection(sectionName);
//        }

//        _currentCutscene = _baseGID2;
//    }

//    public void SkipCutscene(string sectionName = "")
//    {
//        isSkipped = true;
//        _isPlaying = true;
//        //PlayerManager.Instance.ChangeStateToStandart();

//        DeactivateButton(sectionName);

//        if (_baseGID2 != null)
//        {
//            _baseGID2.Stop(Cutscene.StopMode.Rewind);
//            //return;
//        }

//        if (!_isPlaying)
//        {
//            return;
//        }

//        if (_currentCutscene != null)
//        {
//            if (_currentCutscene == _baseGID1)
//            {
//                _currentCutscene.Stop(Cutscene.StopMode.Skip);
//            }
//            else
//            {
//                _currentCutscene.Stop(Cutscene.StopMode.Rewind);
//            }

//            _currentCutscene.Stop();
//        }



//        //        }
//        //        else
//        //        {
//        //            _currentCutscene.Stop(Cutscene.StopMode.Rewind);
//        //        }
//        //
//        Destroy(GameObject.Find("_AudioSources"));
//        ActivateButton();
//        //switch (_currentCutscene.name)
//        //{
//        //    case "BaseGIDCutsceneN1":
//        //    case "BaseGIDCutsceneN2":
//        //        _baseGIDEnd.Play();
//        //        break;
//        //}
//    }

//    private void Update()
//    {

//        if (Input.GetKeyDown(KeyCode.L))
//        {
//            //PlaySectionNow("Test1");
//            NextChapter();
//        }

//        if (Input.GetKeyDown(KeyCode.K))
//        {
//            //PlaySectionNow("Test1");
//            PreviewsChapter();
//        }

//    }

//    public void PlaySectionNow(bool playAllSections = false, string sectionName = "", BtnTap btnTap = null, bool playDemo = false)
//    {
//        //Debug.Log("Click");
//        //        string[] _baseGid1Sections = _baseGID1.GetSectionNames();
//        //        string[] _baseGid2Sections = _baseGID2.GetSectionNames();
//        //        string[] _baseGidEndSections = _baseGIDEnd.GetSectionNames();
//        //
//        baseGid1SectionFound = false;
//        bool _baseGid2SectionFound = false;
//        bool _baseGidEndSectionFound = false;

//        foreach (string item in _baseGid1Sections)
//        {
//            if (sectionName == item)
//            {
//                baseGid1SectionFound = true;
//            }
//        }

//        foreach (string item in _baseGid2Sections)
//        {
//            if (sectionName == item)
//            {
//                _baseGid2SectionFound = true;
//            }
//        }

//        foreach (string item in _baseGidEndSections)
//        {
//            if (sectionName == item)
//            {
//                _baseGidEndSectionFound = true;
//            }
//        }

//        PlayingControll(sectionName, baseGid1SectionFound, _baseGid2SectionFound, _baseGidEndSectionFound, playDemo);
//    }

//    private void PlayingControll(string sectionName, bool _baseGid1SectionFound, bool _baseGid2SectionFound, bool _baseGidEndSectionFound, bool playDemo = false)
//    {
//        Debug.Log("Play control level");
//        if (_baseGid1SectionFound)
//        {
//            _currentCutscene = _baseGID1;
//            //_currentCutscene.PlaySection(sectionName, Cutscene.WrapMode.Once, () => { baseGid1SectionFound = false; Debug.Log("WOOOOOORKS"); ActivateButton(); });
//            isStop = false;
//            StartCoroutine(ChangeChapterDelay(sectionName));
//        }

//        else if (_baseGid2SectionFound)
//        {
//            if (_currentCutscene != _baseGID2)
//            {
//                SkipFirstCutSceneForInit();
//                _currentCutscene = _baseGID2;
//            }
//            isStop = false;
//            //_currentCutscene.PlaySection(sectionName, Cutscene.WrapMode.Once, () => { Debug.Log("WOOOOOORKS"); ActivateButton(); });
//            StartCoroutine(ChangeChapterDelay(sectionName));
//        }

//        if (playDemo)
//        {
//            StartCoroutine(ChangeChapterDelay(sectionName, true));
//        }

//        currentSectionName = sectionName;
//    }

//    public void ChangeCutsceneState()
//    {
//        if (_currentCutscene != null)
//        {
//            _currentCutscene.Stop();
//        }
//    }

//    private void SkipFirstCutSceneForInit()
//    {
//        _baseGID1.Play();
//        _baseGID1.Stop(Cutscene.StopMode.Skip);
//    }

//public void ActivateButton(string sectionName = "", bool fromSharing = false)
//{
//    _isPlaying = true;

//    foreach (BtnTap item in StartScenario.chaptersBtn)
//    {
//        item.ActivateButton(sectionName);
//    }


//    if (!fromSharing)
//        SV_Sharing.Instance.SendBool(true, "activate_menu_items");
//    //foreach (TableElement item in test)
//    //{

//    //    Collider tmp = item.GetComponent<Collider>();
//    //    if (tmp != null)
//    //    {
//    //        tmp.enabled = false;
//    //    }

//    //}
//    //PlayerManager.Instance.ChangeStateToDemonstration();
//}

//public void DeactivateButton(string chapterName = "", bool fromSharing = false)
//{
//    _isPlaying = false;

//    foreach (BtnTap item in StartScenario.chaptersBtn)
//    {
//        item.DeactivateButton(chapterName);
//    }


//    if (!fromSharing)
//        SV_Sharing.Instance.SendBool(true, "deactivate_menu_items");
//    //foreach (TableElement item in test)
//    //{
//    //    Collider tmp = item.GetComponent<Collider>();
//    //    if (tmp != null)
//    //    {
//    //        tmp.enabled = true;
//    //    }
//    //}

//    //PlayerManager.Instance.ChangeStateToStandart();
//}

//    public void NextChapter()
//    {
//        nextChapterForPlay = "";

//        for (int i = 0; i < allSections.Length; i++)
//        {
//            if (allSections[i] == currentSectionName)
//            {
//                if (i == allSections.Length)
//                {
//                    return;
//                }

//                nextChapterForPlay = allSections[i + 1];
//                break;
//            }
//        }

//        StartCoroutine(ChapterDelay());
//    }

//    public void PreviewsChapter()
//    {
//        nextChapterForPlay = "";

//        for (int i = 0; i < allSections.Length; i++)
//        {
//            if (allSections[i] == currentSectionName)
//            {
//                if (i == 0)
//                {
//                    nextChapterForPlay = allSections[0];
//                    return;
//                }
//                nextChapterForPlay = allSections[i - 1];
//                break;
//            }
//        }

//        StartCoroutine(ChapterDelay());
//    }

//    public IEnumerator ChapterDelay()
//    {
//        SkipCutscene();
//        yield return new WaitForSeconds(1f);

//        if (nextChapterForPlay != "")
//        {
//            PlaySectionNow(sectionName: nextChapterForPlay);
//        }
//    }

//    public IEnumerator ChangeChapterDelay(string sectionName, bool isAllSections = false)
//    {

//        if (isAllSections)
//        {
//            SkipCutscene();

//            yield return new WaitForSeconds(.5f);

//            bool canPlay = true;
//            Debug.Log("All sections true");
//            _currentCutscene = _baseGID1;

//            foreach (var item in allSections)
//            {
//                if (canPlay)
//                {
//                    canPlay = false;
//                    isSkipped = false;

//                    _currentCutscene.PlaySection(item, Cutscene.WrapMode.Once, () => Test3(ref canPlay, item));
//                    ActivateButton(item);
//                }

//                yield return new WaitUntil(() => canPlay);
//            }
//        }
//        else
//        {

//            if (!isStop)
//            {
//                SkipCutscene(sectionName);
//                Debug.Log("OOOOOOOOPAAAAAAAAAAA");
//                yield return new WaitForSeconds(.2f);
//                Debug.Log("OOOOOOOOOOOOOOPAAAAAAAAAAAOOOOOOOOPAAAAAAAAAAAOOOOOOOOPAAAAAAAAAAAOOPAAAAAAAAAAA");
//                _currentCutscene.PlaySection(sectionName, Cutscene.WrapMode.Once, () => DeactivateButton());
//                ActivateButton(sectionName);
//            }
//        }
//    }

//    private void Test3(ref bool canPlay, string sectionName)
//    {
//        if (!isSkipped)
//        {
//            canPlay = true;
//            _currentCutscene = _baseGID2;
//            DeactivateButton(sectionName);
//        }
//    }
//}




using UnityEngine;
using System.Collections;
using Slate;

public class CutsceneManager : Singleton<CutsceneManager>
{
    public static bool _isPlaying;

    [SerializeField]
    private Cutscene _baseGID1;
    [SerializeField]
    private Cutscene _baseGID2;
    [SerializeField]
    private Cutscene _baseGIDEnd;
    [SerializeField]
    private Cutscene _utilCutscene;

    private Cutscene _currentCutscene;
    public bool baseGid1SectionFound;
    private bool isSkipped = false;
    private string nextChapterForPlay;
    private string currentSectionName;
    private string[] allSections;
    private string[] _baseGid1Sections;
    private string[] _baseGid2Sections;
    private string[] _baseGidEndSections;
    //public static bool canPlay = true;
    private TableElement[] test;
    public bool isStop;

    private void Test()
    {
        if (!isSkipped)
        {
            Debug.Log("SecondCutscene");
            _baseGID2.Play(() => { Debug.Log("BASE 2"); _baseGIDEnd.Play(() => { ActivateButton(); SkipCutscene(); }); _currentCutscene = _baseGIDEnd; _isPlaying = true; }); _baseGID2.JumpToSection("Groups and periods"); _currentCutscene = _baseGID2;
        }
    }

    void Start()
    {
        Cutscene.OnCutsceneStopped += Cutscene_OnCutsceneStopped;
        _baseGid1Sections = _baseGID1.GetSectionNames();
        _baseGid2Sections = _baseGID2.GetSectionNames();
        _baseGidEndSections = _baseGIDEnd.GetSectionNames();

        allSections = new string[_baseGid1Sections.Length + _baseGid2Sections.Length + _baseGidEndSections.Length];

        for (int i = 0; i < _baseGid1Sections.Length; i++)
        {
            allSections[i] = _baseGid1Sections[i];
        }

        for (int i = 0; i < _baseGid2Sections.Length; i++)
        {
            allSections[i + _baseGid1Sections.Length] = _baseGid2Sections[i];
        }

        for (int i = 0; i < _baseGidEndSections.Length; i++)
        {
            //	allSections [i] = _baseGidEndSections [i];
            allSections[i + _baseGid2Sections.Length + _baseGid1Sections.Length] = _baseGid2Sections[i];
        }

        test = GameObject.FindObjectsOfType<TableElement>();
    }

    private void Cutscene_OnCutsceneStopped(Cutscene obj)
    {
        _isPlaying = false;

        if (obj.name == "BaseGIDCutsceneEnd")
        {
            _baseGIDEnd.RewindNoUndo();
            _baseGID2.RewindNoUndo();
            _baseGID1.RewindNoUndo();
        }
    }

    public void StopCutscene()
    {
        isStop = true;

        if (_currentCutscene != null)
        {
            _currentCutscene.Stop();
            return;
        }

        if (_baseGID1 != null && _baseGID1.isActive)
        {
            _baseGID1.Stop(Cutscene.StopMode.Skip);
            return;
        }

        if (_baseGID2 != null && _baseGID2.isActive)
        {
            _baseGID2.Stop(Cutscene.StopMode.Rewind);
            return;
        }

        if (_baseGIDEnd != null && _baseGIDEnd.isActive)
        {
            _baseGIDEnd.Stop(Cutscene.StopMode.Skip);
            return;
        }
    }

    public void PlayBaseGIDCutscene1()
    {
        isSkipped = false;
        _currentCutscene = _baseGID1;
        _baseGID1.Play(() => { Debug.Log("!PlayBaseGIDCutscene1"); Test(); });
        //DeactivateButton();
    }

    public void PlayBaseGIDCutscene2(string sectionName = "")
    {
        _isPlaying = true;

        _baseGID2.Play();
        if (!string.IsNullOrEmpty(sectionName))
            _baseGID2.JumpToSection(sectionName);

        _currentCutscene = _baseGID2;
    }

    public void SkipCutscene(string sectionName = "")
    {
        isSkipped = true;
        _isPlaying = true;

        DeactivateButton(sectionName);

        if (_baseGID2 != null)
        {
            _baseGID2.Stop(Cutscene.StopMode.SkipRewindNoUndo);
            //return;
        }

        if (!_isPlaying)
        {
            return;
        }

        if (_currentCutscene != null)
        {
            if (_currentCutscene == _baseGID1)
            {
                _currentCutscene.Stop(Cutscene.StopMode.Skip);
            }
            else
            {
                _currentCutscene.Stop(Cutscene.StopMode.Rewind);
            }

            _currentCutscene.Stop();
        }



        //        }
        //        else
        //        {
        //            _currentCutscene.Stop(Cutscene.StopMode.Rewind);
        //        }
        //
        Destroy(GameObject.Find("_AudioSources"));
        ActivateButton();
        //switch (_currentCutscene.name)
        //{
        //    case "BaseGIDCutsceneN1":
        //    case "BaseGIDCutsceneN2":
        //        _baseGIDEnd.Play();
        //        break;
        //}
    }

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.L))
        {
            //PlaySectionNow("Test1");
            NextChapter();
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            //PlaySectionNow("Test1");
            PreviewsChapter();
        }

    }

    public void PlaySectionNow(bool playAllSections = false, string sectionName = "", BtnTap btnTap = null, bool playDemo = false)
    {
        //Debug.Log("Click");
        //        string[] _baseGid1Sections = _baseGID1.GetSectionNames();
        //        string[] _baseGid2Sections = _baseGID2.GetSectionNames();
        //        string[] _baseGidEndSections = _baseGIDEnd.GetSectionNames();
        //
        baseGid1SectionFound = false;
        bool _baseGid2SectionFound = false;
        bool _baseGidEndSectionFound = false;

        foreach (string item in _baseGid1Sections)
        {
            if (sectionName == item)
            {
                baseGid1SectionFound = true;
            }
        }

        foreach (string item in _baseGid2Sections)
        {
            if (sectionName == item)
            {
                _baseGid2SectionFound = true;
            }
        }

        foreach (string item in _baseGidEndSections)
        {
            if (sectionName == item)
            {
                _baseGidEndSectionFound = true;
            }
        }

        PlayingControll(sectionName, baseGid1SectionFound, _baseGid2SectionFound, _baseGidEndSectionFound, playDemo);
    }

    private void PlayingControll(string sectionName, bool _baseGid1SectionFound, bool _baseGid2SectionFound, bool _baseGidEndSectionFound, bool playDemo = false)
    {
        Debug.Log("Play control level");
        if (_baseGid1SectionFound)
        {
            _currentCutscene = _baseGID1;
            //_currentCutscene.PlaySection(sectionName, Cutscene.WrapMode.Once, () => { baseGid1SectionFound = false; Debug.Log("WOOOOOORKS"); ActivateButton(); });
            isStop = false;
            StartCoroutine(ChangeChapterDelay(sectionName));
        }

        else if (_baseGid2SectionFound)
        {
            if (_currentCutscene != _baseGID2)
            {
                SkipFirstCutSceneForInit();
                _currentCutscene = _baseGID2;
            }
            isStop = false;
            //_currentCutscene.PlaySection(sectionName, Cutscene.WrapMode.Once, () => { Debug.Log("WOOOOOORKS"); ActivateButton(); });
            StartCoroutine(ChangeChapterDelay(sectionName));
        }

        if (playDemo)
        {
            StartCoroutine(ChangeChapterDelay(sectionName, true));
        }

        currentSectionName = sectionName;
    }

    public void ChangeCutsceneState()
    {
        if (_currentCutscene != null)
        {
            _currentCutscene.Stop();
        }
    }

    private void SkipFirstCutSceneForInit()
    {
        _baseGID1.Play();
        _baseGID1.Stop(Cutscene.StopMode.Skip);
    }

    public void ActivateButton(string sectionName = "", bool fromSharing = false)
    {
        _isPlaying = true;

        foreach (BtnTap item in StartScenario.chaptersBtn)
        {
            item.ActivateButton(sectionName);
        }


        if (!fromSharing)
            SV_Sharing.Instance.SendBool(true, "activate_menu_items");
        //foreach (TableElement item in test)
        //{

        //    Collider tmp = item.GetComponent<Collider>();
        //    if (tmp != null)
        //    {
        //        tmp.enabled = false;
        //    }

        //}
        //PlayerManager.Instance.ChangeStateToDemonstration();
    }

    public void DeactivateButton(string chapterName = "", bool fromSharing = false)
    {
        _isPlaying = false;

        foreach (BtnTap item in StartScenario.chaptersBtn)
        {
            item.DeactivateButton(chapterName);
        }


        if (!fromSharing)
            SV_Sharing.Instance.SendBool(true, "deactivate_menu_items");
        //foreach (TableElement item in test)
        //{
        //    Collider tmp = item.GetComponent<Collider>();
        //    if (tmp != null)
        //    {
        //        tmp.enabled = true;
        //    }
        //}

        //PlayerManager.Instance.ChangeStateToStandart();
    }

    public void NextChapter()
    {
        nextChapterForPlay = "";

        for (int i = 0; i < allSections.Length; i++)
        {
            if (allSections[i] == currentSectionName)
            {
                if (i == allSections.Length)
                {
                    return;
                }

                nextChapterForPlay = allSections[i + 1];
                break;
            }
        }

        StartCoroutine(ChapterDelay());
    }

    public void PreviewsChapter()
    {
        nextChapterForPlay = "";

        for (int i = 0; i < allSections.Length; i++)
        {
            if (allSections[i] == currentSectionName)
            {
                if (i == 0)
                {
                    nextChapterForPlay = allSections[0];
                    return;
                }
                nextChapterForPlay = allSections[i - 1];
                break;
            }
        }

        StartCoroutine(ChapterDelay());
    }

    public IEnumerator ChapterDelay()
    {
        SkipCutscene();
        yield return new WaitForSeconds(1f);

        if (nextChapterForPlay != "")
        {
            PlaySectionNow(sectionName: nextChapterForPlay);
        }
    }

    public IEnumerator ChangeChapterDelay(string sectionName, bool isAllSections = false)
    {

        if (isAllSections)
        {
            SkipCutscene();

            yield return new WaitForSeconds(.5f);

            bool canPlay = true;
            Debug.Log("All sections true");
            _currentCutscene = _baseGID1;

            foreach (var item in allSections)
            {
                if (canPlay)
                {
                    canPlay = false;
                    isSkipped = false;

                    _currentCutscene.PlaySection(item, Cutscene.WrapMode.Once, () => Test3(ref canPlay, item));
                    ActivateButton(item);
                }

                yield return new WaitUntil(() => canPlay);
            }
        }
        else
        {

            if (!isStop)
            {
                SkipCutscene(sectionName);
                Debug.Log("OOOOOOOOPAAAAAAAAAAA");
                yield return new WaitForSeconds(.2f);
                Debug.Log("OOOOOOOOOOOOOOPAAAAAAAAAAAOOOOOOOOPAAAAAAAAAAAOOOOOOOOPAAAAAAAAAAAOOPAAAAAAAAAAA");
                _currentCutscene.PlaySection(sectionName, Cutscene.WrapMode.Once, () => DeactivateButton());
                ActivateButton(sectionName);
            }
        }
    }

    private void Test3(ref bool canPlay, string sectionName)
    {
        if (!isSkipped)
        {
            canPlay = true;
            _currentCutscene = _baseGID2;
            DeactivateButton(sectionName);
        }
    }
}
