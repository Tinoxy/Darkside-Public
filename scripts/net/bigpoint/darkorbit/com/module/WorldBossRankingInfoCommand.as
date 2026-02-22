package net.bigpoint.darkorbit.com.module
{
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import net.bigpoint.com.module.IModule;
   
   public class WorldBossRankingInfoCommand implements IModule
   {
      private static var _instance:WorldBossRankingInfoCommand;
      
      public var nextRank:WorldBossRankingInfoModule;
      
      public var titleStr:String = "";
      
      public var isActive:Boolean = false;
      
      public var prevRank:WorldBossRankingInfoModule;
      
      public var myRank:WorldBossRankingInfoModule;
      
      public var rankingInfoList:Vector.<WorldBossRankingInfoModule>;
      
      public function WorldBossRankingInfoCommand(param1:Boolean = false, param2:Vector.<WorldBossRankingInfoModule> = null, param3:WorldBossRankingInfoModule = null, param4:WorldBossRankingInfoModule = null, param5:WorldBossRankingInfoModule = null, param6:String = "")
      {
         super();
         if(param3 == null)
         {
            this.nextRank = new WorldBossRankingInfoModule();
         }
         else
         {
            this.nextRank = param3;
         }
         this.titleStr = param6;
         this.isActive = param1;
         if(param5 == null)
         {
            this.prevRank = new WorldBossRankingInfoModule();
         }
         else
         {
            this.prevRank = param5;
         }
         if(param4 == null)
         {
            this.myRank = new WorldBossRankingInfoModule();
         }
         else
         {
            this.myRank = param4;
         }
         if(param2 == null)
         {
            this.rankingInfoList = new Vector.<WorldBossRankingInfoModule>();
         }
         else
         {
            this.rankingInfoList = param2;
         }
      }
      
      public static function get instance() : WorldBossRankingInfoCommand
      {
         return _instance || (_instance = new WorldBossRankingInfoCommand());
      }
      
      public function getLibcomModuleId() : int
      {
         return 9805;
      }
      
      public function getLibcomModuleEstimatedLength() : int
      {
         return 7;
      }
      
      public function read(param1:IDataInput) : void
      {
         var _loc4_:WorldBossRankingInfoModule = null;
         this.nextRank = WorldBossRankingInfoModule(ModuleFactory.get().createInstance(param1.readShort()));
         if(null != this.nextRank)
         {
            this.nextRank.read(param1);
         }
         this.titleStr = param1.readUTF();
         this.isActive = param1.readBoolean();
         this.prevRank = WorldBossRankingInfoModule(ModuleFactory.get().createInstance(param1.readShort()));
         if(null != this.prevRank)
         {
            this.prevRank.read(param1);
         }
         this.myRank = WorldBossRankingInfoModule(ModuleFactory.get().createInstance(param1.readShort()));
         if(null != this.myRank)
         {
            this.myRank.read(param1);
         }
         var _loc2_:int = 0;
         var _loc3_:int = 0;
         while(this.rankingInfoList.length > 0)
         {
            this.rankingInfoList.pop();
         }
         _loc2_ = 0;
         _loc3_ = int(param1.readByte());
         while(_loc2_ < _loc3_)
         {
            _loc4_ = WorldBossRankingInfoModule(ModuleFactory.get().createInstance(param1.readShort()));
            _loc4_.read(param1);
            this.rankingInfoList.push(_loc4_);
            _loc2_++;
         }
      }
      
      public function write(param1:IDataOutput) : void
      {
         var _loc2_:WorldBossRankingInfoModule = null;
         param1.writeShort(9805);
         if(null != this.nextRank)
         {
            this.nextRank.write(param1);
         }
         else
         {
            param1.writeShort(0);
         }
         param1.writeUTF(this.titleStr);
         param1.writeBoolean(this.isActive);
         if(null != this.prevRank)
         {
            this.prevRank.write(param1);
         }
         else
         {
            param1.writeShort(0);
         }
         if(null != this.myRank)
         {
            this.myRank.write(param1);
         }
         else
         {
            param1.writeShort(0);
         }
         param1.writeByte(this.rankingInfoList.length);
         for each(_loc2_ in this.rankingInfoList)
         {
            _loc2_.write(param1);
         }
      }
   }
}

