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
            var win = CreateWindow<UIAbnormalityWindow>("UIAbnormalityWindow", "星球异常");
            return win;
        }

        public void OpenWindow() => Util.OpenWindow(this);

        protected override void _OnCreate()
        {
            _windowTrans = GetRectTransform(this);
            _windowTrans.sizeDelta = new Vector2(600f, 900f);

            CreateUI();
        }

        private void CreateUI()
        {
            var tab1 = new GameObject();
            _tab1 = tab1.AddComponent<RectTransform>();
            NormalizeRectWithMargin(_tab1, 40, 40, 40, 40, _windowTrans);
            tab1.name = "tab-1";

            _nameText = CreateText("星球倾向", 16);
            NormalizeRectWithTopLeft(_nameText.transform, 0f, 20f, _tab1);

            _effectText = CreateText("星球异常", 16);
            NormalizeRectWithTopLeft(_effectText.transform, 0f, 20f, _tab1);

            for (var i = 0; i < _currentEffectCount; ++i) CreateEffectUI(i);
        }

        private void CreateEffectUI(int id)
        {
            var effectBtn = CreateButton("Effect" + id, 40, 20);
            _effectBtns[id] = effectBtn;
            NormalizeRectWithTopLeft(effectBtn.transform, 0, 60 + id * 30, _tab1);

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

            // TODO: rewrite this

            var vector = new Vector2(_effectText.preferredWidth, _effectText.preferredHeight);
            vector.x = (int)((vector.x + 2.0) / 2.0) * 2;
            vector.y = (int)((vector.y + 2.0) / 2.0) * 2;
            _effectText.rectTransform.sizeDelta = vector;

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
                NormalizeRectWithTopLeft(effectBtn.transform, 40, _effectText.rectTransform.anchoredPosition.y + 60 + i * 60, _tab1);
            }

            for (var i = abnormality.Effects.Length; i < effectBtnsLength; ++i) _effectBtns[i].gameObject.SetActive(false);

            _windowTrans.sizeDelta = new Vector2(Mathf.Max(120, vector.x + 80), Mathf.Max(240, vector.y + 100 + abnormality.Effects.Length * 30));
            NormalizeRectWithMargin(_tab1, 40, 40, 40, 40, _windowTrans);

            OpenWindow();
        }

        private void OnBtnClick(int id)
        {
            var effect = _currentEffects[id];
            switch (effect.Type)
            {
                case EffectType.AddItem:
                    var item = LDB.items.Select(effect.Value[0]);
                    GameMain.mainPlayer.TryAddItemToPackage(item.ID, effect.Value[1], 0, false);
                    break;
            }

            _Close();
        }
    }
}
