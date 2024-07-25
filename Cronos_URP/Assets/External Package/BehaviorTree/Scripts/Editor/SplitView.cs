using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// 'SplitView' 클래스는 'TwoPaneSplitView' 의 기능을 상속받아, 두 개의 주요 영역이나 패널을 가진 분할 뷰를 UI에 구현하는 데 사용된다.
/// 이 클래스 자체는 상속받은 기능 외에 추가적인 구현이나 멤버 변수를 정의하지 않고 있다.
/// 그러나 이 클래스를 사용함으로써, 'TwoPaneSplitView' 에 없는 기능을 추가하거나, 기존 기능을 오버라이드하여 사용자 정의 동작을 구현할 수 있는 기반을 마련할 수 있다.
/// </summary>
public class SplitView : TwoPaneSplitView
{
    public new class UxmlFactory : UxmlFactory<SplitView, TwoPaneSplitView.UxmlTraits> { }

    /*
     * public new class UxmlFactory : UxmlFactory<SplitView, TwoPaneSplitView.UxmlTraits> { }
     * 이 내부 클래스는 'UxmlFactory' 를 상속받아, UI Toolkit의 UXML 파일에서 'SplitView' 요소를 사용할 수 있게 해준다.
     * 이를 통해 'SplitView' 요소를 UXML 파일 내에서 직접 사용할 수 있게 되며,
     * UI Toolkit을 사용한 UI 구성 시 'SplitView' 커스텀 컴포넌트를 효과적으로 활용할 수 있다.
     */
}
