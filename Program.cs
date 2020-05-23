using System;

namespace MC基岩版物品配方转方块配方工具
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("请输入目标mod行为包路径");
            var sourcebpath = /*Console.ReadLine();*/@"H:\server\mcpe\mods\behaviour_packs\MoreFoodBP v009";
            Console.WriteLine("请输入目标mod材质包路径");
            var sourcerpath = /*Console.ReadLine()*/@"H:\server\mcpe\mods\resource_packs\MoreFoodRP v009";
            Console.WriteLine("请输入生成Mod行为包路径");
            var generatebpath = /*Console.ReadLine()*/"";
            Console.WriteLine("请输入生成Mod材质包路径");
            var generaterpath  = /*Console.ReadLine()*/"";
            var tool = new ModTool(sourcebpath,sourcerpath,generatebpath,generaterpath);
            tool.ReadFile();
        }
    }
}
