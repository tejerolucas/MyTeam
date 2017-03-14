//  Copyright 2016 MaterialUI for Unity http://materialunity.com
//  Please see license file for terms and conditions of use, and more information.

using MaterialUI;
using UnityEngine.UI;
using UnityEngine;

public class Example07LengthValidation : MonoBehaviour, ITextValidator
{
	private MaterialInputField m_MaterialInputField;

	public void Init(MaterialInputField materialInputField)
	{
		m_MaterialInputField = materialInputField;
	}

	public bool IsTextValid ()
	{
		if (m_MaterialInputField.inputField.text.Contains ("@etermax.com")) {
			return true;
		} else {
			m_MaterialInputField.validationText.text = "No es un email de Etermax valido";
            return false;
		}
    }
}