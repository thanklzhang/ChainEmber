using Battle;
using Battle_Client;
using Config;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using BattleBox = Config.BattleBox;

//战斗当前波结算UI 中的宝箱
public class BattleWavePassUI_BoxCell
{
   private GameObject gameObject;
   private Transform transform;

   private WavePassRewardBoxUIGroupData data;
   private BattleWavePassUI wavePassUI;
   
   
   private Image iconImg;
   private Image iconBgImg;
   private TextMeshProUGUI countText;
   
   public void Init(GameObject go,BattleWavePassUI wavePassUI)
   {
      this.gameObject = go;
      this.transform = this.gameObject.transform;
      this.wavePassUI = wavePassUI;
      
      iconImg = this.transform.Find("iconBg/icon").GetComponent<Image>();
      countText = this.transform.Find("count_text").GetComponent<TextMeshProUGUI>();
      iconBgImg = this.transform.Find("iconBg").GetComponent<Image>();
   }

   public void Show()
   {
      this.gameObject.SetActive(true);
   }

   public void RefreshUI(WavePassRewardBoxUIGroupData data)
   {
      this.data = data;
      
      var config = ConfigManager.Instance.GetById<BattleBox>(this.data.boxConfigId); 
      var resId = config.IconResId;
      ResourceManager.Instance.GetObject<Sprite>(resId, (sprite) =>
      {
         this.iconImg.sprite = sprite;
      });

      countText.text = "x" + this.data.count;
      iconBgImg.color = ColorDefine.GetColorByQuality((RewardQuality)config.Quality);
   }

   public void Hide()
   {
      this.gameObject.SetActive(false);
   }

   public void Release()
   {
      
   }
}