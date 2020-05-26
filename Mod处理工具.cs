using System;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MC基岩版物品配方转方块配方工具
{
    public class ModTool
    {
        /// <summary>
        /// 源mod行为包路径
        /// </summary>
        public string SBPath;
        /// <summary>
        /// 源mod材质包路径
        /// </summary>
        public string SRPath;
        /// <summary>
        /// 生成Mod行为包路径
        /// </summary>
        public string TBPath;
        /// <summary>
        /// 生成Mod材质包路径
        /// </summary>
        public string TRPath;

        /// <summary>
        /// 源mod行为包中物品目录
        /// </summary>
        protected string SBItemPath;
        /// <summary>
        /// 源mod行为包中合成配方目录
        /// </summary>
        protected string SBRecipesPath;
        /// <summary>
        /// 源Mod材质包中贴图目录
        /// </summary>
        protected string SRTexturesPath;
        /// <summary>
        /// 源mod材质包中物品贴图目录
        /// </summary>
        protected string SRTItemsPath;

        /// <summary>
        /// 生成Mod行为包中方块目录
        /// </summary>
        protected string TBBlockPath;
        /// <summary>
        /// 生成Mod行为包中合成配方目录
        /// </summary>
        protected string TBRecipesPath;
        /// <summary>
        /// 生成Mod行为包中掉落表目录
        /// </summary>
        protected string TBLootPath;

        /// <summary>
        /// 生成Mod材质包中记录Blocks贴图信息的json
        /// </summary>
        protected string TRBlocksJsonPath;
        /// <summary>
        /// 生成的Mod材质包中方块贴图目录
        /// </summary>
        protected string TRTBlocksPath;



        /// <summary>
        /// 源mod行为包物品的Json集合
        /// </summary>
        protected Dictionary<ObjectName,JObject> SourceItemsJson;

        protected List<JObject> SourceRecipesJson;

        public ModTool(string sbPath, string srPath, string tbPath, string trPath){
            SBPath = sbPath;
            SRPath = srPath;
            TBPath = tbPath;
            TRPath = trPath;
            
            SBItemPath = SBPath + @"\items";
            SBRecipesPath = SBPath + @"\recipes";

            SRTexturesPath = SRPath + @"\textures";
            SRTItemsPath = SRPath + @"\item";

            TBBlockPath = TBPath + @"\blocks";
            TBRecipesPath = TBPath + @"\recipes";
            TBLootPath = TBPath + @"\loot_tables";
            
            TRBlocksJsonPath = TRPath + "blocks.json";
            TRTBlocksPath = TRPath + @"\textures\blocks";
        }
        /// <summary>
        /// 加载源mod内需要的文件
        /// </summary>
        /// <returns></returns>
        public void ReadFile(){
            //加载所有物品文件
            string[] files = Directory.GetFiles(SBItemPath,"*.json",SearchOption.AllDirectories);
            SourceItemsJson = new Dictionary<ObjectName, JObject>();
            for (int i = 0; i < files.Length;i++){
                try{
                    var json = JObject.Parse(File.ReadAllText(files[i]));
                    var Name = GetItemName(json);
                    System.Console.WriteLine(Name.FullName);
                    SourceItemsJson.Add(Name,json);
                }catch(Exception e){
                    Console.WriteLine("错误：" + e.ToString());
                    Console.WriteLine("路径：" + files[i]);
                }
            }
            Console.WriteLine("共找到{0}个物品文件",SourceItemsJson.Count);
            //加载所有配方文件
            files = Directory.GetFiles(SBRecipesPath,"*.json",SearchOption.AllDirectories);
            SourceRecipesJson = new List<JObject>();
            for (int i = 0; i < files.Length;i++){
                try{
                    var recipe = JObject.Parse(File.ReadAllText(files[i]));
                    if(IsCrafting(recipe)){
                        SourceRecipesJson.Add(recipe);
                        System.Console.WriteLine(recipe["minecraft:recipe_shaped"]["description"]["identifier"].ToString());
                    }
                }catch(Exception e){
                    Console.WriteLine("错误：" + e.ToString());
                    Console.WriteLine("序数：" + i);
                    Console.WriteLine("路径：" + files[i]);
                }
            } 
            Console.WriteLine("共找到{0}个配方文件",SourceRecipesJson.Count);

        }
        /// <summary>
        /// 从物品json文件中获得物品的名称信息（带命名空间名）
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public ObjectName GetItemName(JObject obj){
            return new ObjectName(){FullName = obj["minecraft:item"]["description"]["identifier"].ToString()};
        }
        /// <summary>
        /// 是否是需要手工合成的配方
        /// </summary>
        public bool IsCrafting(JObject Recipe){
            if(Recipe.ContainsKey("minecraft:recipe_shaped")){
                if(Recipe["minecraft:recipe_shaped"]["tags"].ToObject<string[]>()[0] == "crafting_table"){//待修改匹配规则
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// 获取合成配方的产物（多产物只返回有价值的第一个）
        /// </summary>
        public ObjectName GetRecipeResult(JObject Recipe){
            return default(ObjectName);
        }
    }
    public struct ObjectName{
        public string FullName{get => _fullName;set{
            _fullName = value;
            string[] names = value.Split(":");
            if(names.Length != 2){
                System.Console.WriteLine("名称长度不符合预期！"+ names.Length);
                throw new Exception("名称长度不符合预期");
            }
            NamespaceName = names[0];
            Name = names[1];
        }}

        private string _fullName;
        public string NamespaceName{get;private set;}
        public string Name{get;private set;}
    }
}