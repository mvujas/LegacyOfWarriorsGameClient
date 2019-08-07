using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CardSpriteCatalogue : MonoBehaviourWithAddOns
{
    [Serializable]
    public class CatalogueEntry
    {
        public string spriteKey;
        public Sprite sprite;
    }

    [SerializeField]
    private Sprite defaultSprite = null;
    [SerializeField]
    private CatalogueEntry[] catalogueEntries = { };

    private Dictionary<string, Sprite> m_catalogueMapper = new Dictionary<string, Sprite>();

    private void Awake()
    {
        if (defaultSprite == null)
        {
            throw new ArgumentNullException(nameof(defaultSprite));
        }
        foreach(var entry in catalogueEntries)
        {
            if(entry.sprite != null && !m_catalogueMapper.ContainsKey(entry.spriteKey))
            {
                m_catalogueMapper.Add(entry.spriteKey, entry.sprite);
            }
        }
        globalReference.SpriteCatalogue = this;
    }

    public bool GetSprite(string spriteKey, out Sprite sprite)
    {
        if(m_catalogueMapper.TryGetValue(spriteKey, out sprite))
        {
            return true;
        }
        sprite = defaultSprite;
        return false;
    }
}
