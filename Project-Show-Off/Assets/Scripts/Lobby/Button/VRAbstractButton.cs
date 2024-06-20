using UnityEngine;

public abstract class VRAbstractButton : MonoBehaviour
{
    [SerializeField] float buttonPressDepth = 1.0f;
    [SerializeField] float buttonPressDuration = 0.5f;
    [SerializeField] float buttonResetDuration = 0.3f;

    [SerializeField] Animator glass;

    private int _currentTweenId = -1;
    private Vector3 _buttonStartPos;
    private Vector3 _buttonEndPos;
    private float _tweenMaxDistance;

    private bool _glassOpen;

    SoundPlayer _soundPlayer;

    private void Start()
    {
        Initialize();
        _soundPlayer = GetComponent<SoundPlayer>();
    }

    protected virtual void Initialize()
    {
        // Set the buttonStartPos to transform.position, as it's attached to the button...
        _buttonStartPos = transform.position;
        _buttonEndPos = _buttonStartPos - (transform.up * buttonPressDepth);

        // Get the distance in order to lerp the tween duration properly...
        _tweenMaxDistance = (_buttonEndPos - _buttonStartPos).magnitude;

        if (glass == null)
        {
            _glassOpen = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log(_glassOpen);   
        if ((other.tag == "RightController" || other.tag == "LeftController")&& _glassOpen)
        {
            // Move the button back...
            TweenButton();
        }
        if(other.tag == "Hammer" && !_glassOpen)
        {
            AnimateGlass();
            _glassOpen = true; 
            EventBus<GlassBrokenEvent>.Publish(new GlassBrokenEvent());
        }
    }

    private void AnimateButton()
    {
        // Open glass...
        if (!_glassOpen && glass)
        {
           // AnimateGlass();
           // _glassOpen = true;
        }
        // Press button default..
        else { TweenButton(); }
    }

    private void TweenButton()
    {
        _soundPlayer.Play();
        Debug.Log("button tween");
        if (_currentTweenId == -1)
        {
            // Move the button back according to buttonPressDepth...
            _currentTweenId = LeanTween.move(gameObject, _buttonEndPos, buttonPressDuration).setOnComplete(() =>
            {
                OnButtonPress();
                // Move back to the original position
                _currentTweenId = LeanTween.move(gameObject, _buttonStartPos, buttonResetDuration).setOnComplete(() =>
                {
                    _currentTweenId = -1;
                }).id;
            }).id;
        }
        // Press button if in the middle of tween...
        else
        {
            LeanTween.cancel(_currentTweenId);
            float adjustedTime = GetTweenTimeFactor((_buttonEndPos - transform.position).magnitude);

            _currentTweenId = LeanTween.move(gameObject, _buttonEndPos, buttonPressDuration * adjustedTime).setOnComplete(() =>
            {
                OnButtonPress();
                _currentTweenId = LeanTween.move(gameObject, _buttonStartPos, buttonResetDuration).setOnComplete(() =>
                {
                    _currentTweenId = -1;
                }).id;
            }).id;
        }
    }

    private float GetTweenTimeFactor(float pCurrentDistance)
    {
        // Get a factor depending on how close the currentDistance is to the max...
        // This is used to change the time needed to finish the tween according to the distance...
        pCurrentDistance = Mathf.Clamp(pCurrentDistance, 0f, _tweenMaxDistance);
        float normalizedValue = pCurrentDistance / _tweenMaxDistance;
        return 1.0f - normalizedValue;
    }

    private void AnimateGlass()
    {
        glass.SetTrigger("Break");
    }


    public abstract void OnButtonPress();

}
