using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;

public class PlayerController : MonoBehaviour
{
    public Hittable hp;
    public HealthBar healthui;
    public ManaBar manaui;

    public SpellCaster spellcaster;
    public SpellUI spellui;

    public int speed;

    public Unit unit;
    private Coroutine manaRoutine;

    void Start()
    {
        unit = GetComponent<Unit>();
        GameManager.Instance.player = gameObject;
    }

    public void StartLevel()
    {
        unit.movement = Vector2.zero;
        if (manaRoutine != null)
        {
            StopCoroutine(manaRoutine);
        }

        Dictionary<string, int> d = GameManager.Instance.dict;
        if (!d.ContainsKey("wave")) d.Add("wave", 1);
        else d["wave"] = 1;

        spellcaster = new SpellCaster(1, 1, Hittable.Team.PLAYER);
        manaRoutine = StartCoroutine(spellcaster.ManaRegeneration());

        hp = new Hittable(1, Hittable.Team.PLAYER, gameObject);
        hp.OnDeath += Die;
        hp.team = Hittable.Team.PLAYER;

        ScaleStats(ClassInfo.Instance.GetClass(ClassInfo.Instance.selectedClass));

        healthui.SetHealth(hp);
        manaui.SetSpellCaster(spellcaster);
        //spellui.SetSpell(spellcaster.spell);
        spellui.SetSpell(spellcaster.spell, spellcaster.activeSpellIndex);
    }

    public void ScaleStats(int wave)
    {
        Dictionary<string, int> d = GameManager.Instance.dict;

        JToken classData = ClassInfo.Instance.classData[ClassInfo.Instance.selectedClass];

        int maxHP = RPNEvaluator.RPNEvaluator.Evaluate(classData["health"].ToString(), d);
        int maxMana = RPNEvaluator.RPNEvaluator.Evaluate(classData["mana"].ToString(), d);
        int manaReg = RPNEvaluator.RPNEvaluator.Evaluate(classData["mana_regeneration"].ToString(), d);
        int spellPower = RPNEvaluator.RPNEvaluator.Evaluate(classData["spellpower"].ToString(), d);
        speed = RPNEvaluator.RPNEvaluator.Evaluate(classData["speed"].ToString(), d);

        //int maxHP = RPNEvaluator.RPNEvaluator.Evaluate("95 wave 5 * +", d);
        //int maxMana = RPNEvaluator.RPNEvaluator.Evaluate("90 wave 10 * +", d);
        //int manaReg = RPNEvaluator.RPNEvaluator.Evaluate("10 wave +", d);
        //int spellPower = RPNEvaluator.RPNEvaluator.Evaluate("wave 10 *", d);
        //speed = RPNEvaluator.RPNEvaluator.Evaluate("5", d);

        hp.SetMaxHP(maxHP);

        spellcaster.max_mana = maxMana;
        spellcaster.mana = Mathf.Min(spellcaster.mana, maxMana);
        spellcaster.mana_reg = manaReg;
        spellcaster.power = spellPower;
        
        //Debug.Log("Class: " + ClassInfo.Instance.selectedClass +
        //  " HP: " + maxHP +
        //  " Mana: " + maxMana +
        //  " ManaReg: " + manaReg +
        //  " Power: " + spellPower +
        //  " Speed: " + speed);

        healthui.SetHealth(hp);
    }

    public void ScaleStats(JToken PlayerClass)
    {
        Dictionary<string, int> d = GameManager.Instance.dict;
        if (PlayerClass == null) return;
        JProperty pClass = (JProperty)PlayerClass;
        //att["sprite"];
        if(pClass.Value["health"] != null) hp.SetMaxHP(RPNEvaluator.RPNEvaluator.Evaluate(pClass.Value["health"].ToString(), d));
        if (pClass.Value["mana"] != null) spellcaster.max_mana = RPNEvaluator.RPNEvaluator.Evaluate(pClass.Value["mana"].ToString(), d);
        if (pClass.Value["mana"] != null) spellcaster.mana = Mathf.Min(spellcaster.mana, spellcaster.max_mana);
        if (pClass.Value["mana_regeneration"] != null) spellcaster.mana_reg = RPNEvaluator.RPNEvaluator.Evaluate(pClass.Value["mana_regeneration"].ToString(), d);
        if (pClass.Value["spellpower"] != null) spellcaster.power = RPNEvaluator.RPNEvaluator.Evaluate(pClass.Value["spellpower"].ToString(), d);
        if (pClass.Value["speed"] != null) speed = RPNEvaluator.RPNEvaluator.Evaluate(pClass.Value["speed"].ToString(), d);
    }

    void Update()
    {

    }

    void OnNext(InputValue value)
    {
        if (spellcaster == null) return;
        spellcaster.NextSpell();
        //spellui.SetSpell(spellcaster.spell);
        spellui.SetSpell(spellcaster.spell, spellcaster.activeSpellIndex);
    }

    void OnPrevious(InputValue value)
    {
        if (spellcaster == null) return;
        spellcaster.PrevSpell();
        //spellui.SetSpell(spellcaster.spell);
        spellui.SetSpell(spellcaster.spell, spellcaster.activeSpellIndex);
    }

    void OnAttack(InputValue value)
    {
        if (GameManager.Instance.state != GameManager.GameState.INWAVE) return;
        Vector2 mouseScreen = Mouse.current.position.value;
        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(mouseScreen);
        mouseWorld.z = 0;
        StartCoroutine(spellcaster.Cast(transform.position, mouseWorld));
    }

    void OnMove(InputValue value)
    {
        if (GameManager.Instance.state != GameManager.GameState.COUNTDOWN &&
            GameManager.Instance.state != GameManager.GameState.INWAVE)
        {
            unit.movement = Vector2.zero;
            return;
        }
        unit.movement = value.Get<Vector2>() * speed;
    }

    void Die()
    {
        unit.movement = Vector2.zero;
        GameManager.Instance.resultMessage = "You were defeated on wave " + GameManager.Instance.wave + ".";
        GameManager.Instance.state = GameManager.GameState.GAMEOVER;
    }
}