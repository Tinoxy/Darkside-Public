package net.bigpoint.darkorbit.com.module
{
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import net.bigpoint.com.module.IModule;
   
   public class BattlePassUpdateStatusCommand implements IModule
   {
      private static var _instance:BattlePassUpdateStatusCommand;
      
      public var purchased:Boolean = false;
      
      public var questsToCollectReward:Vector.<int>;
      
      public function BattlePassUpdateStatusCommand(param1:Boolean = false, param2:Vector.<int> = null)
      {
         super();
         this.purchased = param1;
         if(param2 == null)
         {
            this.questsToCollectReward = new Vector.<int>();
         }
         else
         {
            this.questsToCollectReward = param2;
         }
      }
      
      public static function get instance() : BattlePassUpdateStatusCommand
      {
         return _instance || (_instance = new BattlePassUpdateStatusCommand());
      }
      
      public function getLibcomModuleId() : int
      {
         return 31259;
      }
      
      public function getLibcomModuleEstimatedLength() : int
      {
         return 5;
      }
      
      public function read(param1:IDataInput) : void
      {
         var _loc4_:* = 0;
         this.purchased = param1.readBoolean();
         var _loc2_:int = 0;
         var _loc3_:int = 0;
         while(this.questsToCollectReward.length > 0)
         {
            this.questsToCollectReward.pop();
         }
         _loc2_ = 0;
         _loc3_ = int(param1.readShort());
         while(_loc2_ < _loc3_)
         {
            _loc4_ = int(param1.readInt());
            _loc4_ = _loc4_ << 14 | _loc4_ >>> 18;
            this.questsToCollectReward.push(_loc4_);
            _loc2_++;
         }
      }
      
      public function write(param1:IDataOutput) : void
      {
         var _loc2_:int = 0;
         param1.writeShort(31259);
         param1.writeBoolean(this.purchased);
         param1.writeShort(this.questsToCollectReward.length);
         for each(_loc2_ in this.questsToCollectReward)
         {
            param1.writeInt(_loc2_ >>> 14 | _loc2_ << 18);
         }
      }
   }
}

