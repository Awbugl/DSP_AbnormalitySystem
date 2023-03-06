using System.Collections.Generic;
using System.Linq;
using DSP_AbnormalitySystem.Abnormality;
using UnityEngine;
using UnityEngine.UI;
using static DSP_AbnormalitySystem.UI.Util;

namespace DSP_AbnormalitySystem.UI
{
    /// <summary>
    ///   special thanks to https://github.com/hetima/DSP_PlanetFinder/tree/main/PlanetFinder
    ///   special thanks to https://github.com/starfi5h/DSP_Mod_Support/tree/main/FactoryLocator/src/UI
    /// </summary>
    public class UIAbnormalityWindow : ManualBehaviour
    {
        private RectTransform _windowTrans;
        private Text _nameText;

        private RectTransform _tab1;

        private int _currentEffectCount = 4;

        private UIButton[] _effectBtns = new UIButton[4];
        private Text _effectText;
        private Effect[] _currentEffects;

        internal static UIAbnormalityWindow CreateWindow()
        {
            var win = CreateWindow<UIAbnormalityWindow>("UIAbnormalityWindow", Localization.language == Language.zhCN ? "异常" : "Abnormality");
            return win;
        }

        public void OpenWindow() => Util.OpenWindow(this);

        protected override void _OnCreate()
        {
            _windowTrans = this.GetRectTransform();
            _windowTrans.sizeDelta = new Vector2(480f, 500f);

            CreateUI();
        }

        private void CreateUI()
        {
            var tab1 = new GameObject();
            _tab1 = tab1.AddComponent<RectTransform>();
            tab1.name = "tab-1";

            _nameText = CreateText("_nameText", 16);

            _effectText = CreateText("_effectText", 16);
            _effectText.alignment = TextAnchor.UpperLeft;
            _effectText.horizontalOverflow = HorizontalWrapMode.Wrap;

            for (var i = 0; i < _currentEffectCount; ++i) CreateEffectUI(i);
        }

        private void CreateEffectUI(int id)
        {
            var effectBtn = CreateButton("Effect" + id, 40, 20);
            _effectBtns[id] = effectBtn;
            effectBtn.onClick += _ => OnBtnClick(id);
        }

        protected override void _OnUpdate()
        {
            if (VFInput.escape)
            {
                VFInput.UseEscape();
                _Close();
            }
        }

        public void SetAbnormality(PlanetData planet, Abnormality.Abnormality abnormality)
        {
            Dictionary<string, StringProto> trans = abnormality.Translations.ToDictionary(i => i.Name);
            var lang = Localization.language;

            string Translate(string s)
            {
                var proto = trans[s];

                switch (lang)
                {
                    case Language.zhCN:
                        return proto.ZHCN;

                    case Language.enUS:
                        return proto.ENUS;

                    case Language.frFR:
                        return proto.FRFR ?? proto.ENUS;
                }

                return s;
            }

            _nameText.text = planet.displayName + " - " + Translate(abnormality.Name);
            _effectText.text = Translate(abnormality.Description);
            _currentEffects = abnormality.Effects;
            _currentEffectCount = abnormality.Effects.Length;

            var y = (int)((_effectText.preferredHeight + 2.0) / 2.0) * 2;

            _windowTrans.SetRectTransformSize(new Vector2(480f, y + 120 + abnormality.Effects.Length * 30));
            _tab1.NormalizeRectWithMargin(40, 40, 40, 40, false, _windowTrans);
            _nameText.transform.NormalizeRectWithTopLeft(0, 20, _tab1);
            _effectText.transform.NormalizeRectWithTopLeft(0, 60, _tab1);
            _effectText.rectTransform.SetRectTransformSize(new Vector2(400f, y));

            var effectBtnsLength = _effectBtns.Length;

            if (effectBtnsLength < _currentEffectCount)
            {
                _effectBtns = new UIButton[_currentEffectCount];

                for (var i = effectBtnsLength; i < _currentEffectCount; ++i) CreateEffectUI(i);
            }

            for (var i = 0; i < abnormality.Effects.Length; ++i)
            {
                var effectBtn = _effectBtns[i];
                effectBtn.gameObject.SetActive(true);
                var abnormalityEffect = abnormality.Effects[i];
                var tipsTipTitle = Translate(abnormalityEffect.Name);
                effectBtn.SetUIButtonText(tipsTipTitle);
                effectBtn.tips.tipText = Translate(abnormalityEffect.Description);
                effectBtn.tips.tipTitle = tipsTipTitle;
                effectBtn.UpdateTip();
                effectBtn.transform.NormalizeRectWithTopLeft(0, y + 40 + i * 30, _tab1);
                var btnSize = new Vector2(400f, 25f);
                ((RectTransform)effectBtn.transform).SetRectTransformSize(btnSize);
            }

            for (var i = abnormality.Effects.Length; i < effectBtnsLength; ++i) _effectBtns[i].gameObject.SetActive(false);

            OpenWindow();
        }

        private void OnBtnClick(int id)
        {
            var effect = _currentEffects[id];

            // ReSharper disable once ForeachCanBePartlyConvertedToQueryUsingAnotherGetEnumerator
            foreach (KeyValuePair<EffectType, AbnormalitySystem.Awards> pair in AbnormalitySystem.AwardsMap)
            {
                if ((effect.Type & pair.Key) > 0) pair.Value(effect.Value[pair.Key]);
            }

            _Close();
        }
    }
}
