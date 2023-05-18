using I2.Loc;
using Sirenix.OdinInspector;
using UnityEngine;

namespace TeamTheDream
{
    [System.Serializable]
    public class DialogueText
    {
        [SerializeField, Space, TermsPopup("Dialogue"), OnValueChanged("ShowEditorTexts")]
        private string _term;

#if UNITY_EDITOR
        [SerializeField, HideInInspector]
        private Object _context;

        [SerializeField, Indent, Multiline, LabelText("- Español")]
        string _textES;

        [SerializeField, Indent, Multiline, LabelText("- English")]
        string _textEN; 
        private void ShowEditorTexts()
        {
            _textES = LocalizationManager.GetTranslation(_term, true, 0, true, false, null, "Spanish");
            _textEN = LocalizationManager.GetTranslation(_term, true, 0, true, false, null, "English");
        }

        [SerializeField, BoxGroup("Create"), PropertyOrder(-1), InlineButton("CreateNewTermFromEditor", "Create new term")]
        string _newTerm;

        private void CreateNewTermFromEditor()
        {
            if (!string.IsNullOrEmpty(_newTerm))
            {
                string term = "Dialogue/" + _newTerm;

                TermData termData = LocalizationManager.Sources[0].AddTerm(term);
                termData.SetTranslation(LocalizationManager.Sources[0].GetLanguageIndex("Spanish"), _textES);
                termData.SetTranslation(LocalizationManager.Sources[0].GetLanguageIndex("English"), _textEN);

                LocalizationManager.Sources[0].UpdateDictionary();
                _term = term;
                _newTerm = null;

                UnityEditor.EditorUtility.SetDirty(_context);
            }
        }

        [Button("Save localization"), Indent]
        private void UpgradeTextsFromEditor()
        {
            TermData termData = LocalizationManager.GetTermData(_term);
            termData.SetTranslation(LocalizationManager.Sources[0].GetLanguageIndex("Spanish"), _textES);
            termData.SetTranslation(LocalizationManager.Sources[0].GetLanguageIndex("English"), _textEN);

            LocalizationManager.Sources[0].UpdateDictionary();
        }
        
        [Button("Load localization"), Indent]
        private void LoadTextsFromEditor()
        {
            TermData termData = LocalizationManager.GetTermData(_term);
            _textES = termData.GetTranslation(LocalizationManager.Sources[0].GetLanguageIndex("Spanish"));
            _textEN = termData.GetTranslation(LocalizationManager.Sources[0].GetLanguageIndex("English"));
            
            UnityEditor.EditorUtility.SetDirty(_context);
        }

        [Button]
        public void SetContext(Object context)
        {
            _context = context;
        }
#endif

        public string Text => LocalizationManager.GetTranslation(_term);
    }
}
