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
        protected List<JObject> SourceItemsJson;

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
            SourceItemsJson = new List<JObject>(files.Length);
            for (int i = 0; i < files.Length;i++){
                try{
                    SourceItemsJson.Add(JObject.Parse(File.ReadAllText(files[i])));
                }catch(Exception e){
                    Console.WriteLine("错误：" + e.ToString());
                    Console.WriteLine("序数：" + i);
                    Console.WriteLine("路径：" + files[i]);
                }
            }
            Console.WriteLine("共找到{0}个物品文件",SourceItemsJson.Count);
            //加载所有配方文件
            files = Directory.GetFiles(SBRecipesPath,"*.json",SearchOption.AllDirectories);
            SourceRecipesJson = new List<JObject>();
            for (int i = 0; i < files.Length;i++){
                try{
                    SourceRecipesJson.Add(JObject.Parse(File.ReadAllText(files[i])));
                }catch(Exception e){
                    Console.WriteLine("错误：" + e.ToString());
                    Console.WriteLine("序数：" + i);
                    Console.WriteLine("路径：" + files[i]);
                }
            } 
            Console.WriteLine("共找到{0}个配方文件",SourceRecipesJson.Count);

        }

    }
}