package net.bigpoint.darkorbit.com.module
{
   import flash.utils.IDataInput;
   import flash.utils.IDataOutput;
   import net.bigpoint.com.module.IModule;
   
   public class MonthlyDeluxeDayInfoCommand implements IModule
   {
      private static var _instance:MonthlyDeluxeDayInfoCommand;
      
      public var today:int = 0;
      
      public var daysClaimed:Vector.<int>;
      
      public var isClaimable:Boolean = false;
      
      public function MonthlyDeluxeDayInfoCommand(param1:Vector.<int> = null, param2:int = 0, param3:Boolean = false)
      {
         super();
         this.today = param2;
         if(param1 == null)
         {
            this.daysClaimed = new Vector.<int>();
         }
         else
         {
            this.daysClaimed = param1;
         }
         this.isClaimable = param3;
      }
      
      public static function get instance() : MonthlyDeluxeDayInfoCommand
      {
         return _instance || (_instance = new MonthlyDeluxeDayInfoCommand());
      }
      
      public function getLibcomModuleId() : int
      {
         return -16109;
      }
      
      public function getLibcomModuleEstimatedLength() : int
      {
         return 7;
      }
      
      public function read(param1:IDataInput) : void
      {
         var _loc4_:* = 0;
         this.today = param1.readShort();
         this.today = 65535 & ((65535 & this.today) >>> 12 | (65535 & this.today) << 4);
         this.today = this.today > 32767 ? int(this.today - 65536) : this.today;
         var _loc2_:int = 0;
         var _loc3_:int = 0;
         while(this.daysClaimed.length > 0)
         {
            this.daysClaimed.pop();
         }
         _loc2_ = 0;
         _loc3_ = int(param1.readByte());
         while(_loc2_ < _loc3_)
         {
            _loc4_ = int(param1.readShort());
            _loc4_ = 65535 & ((65535 & _loc4_) << 15 | (65535 & _loc4_) >>> 1);
            _loc4_ = (65535 & ((65535 & _loc4_) << 15 | (65535 & _loc4_) >>> 1)) > 32767 ? int(_loc4_ - 65536) : _loc4_;
            this.daysClaimed.push(_loc4_);
            _loc2_++;
         }
         this.isClaimable = param1.readBoolean();
      }
      
      public function write(param1:IDataOutput) : void
      {
         var _loc2_:int = 0;
         param1.writeShort(-16109);
         param1.writeShort(65535 & ((65535 & this.today) << 12 | (65535 & this.today) >>> 4));
         param1.writeByte(this.daysClaimed.length);
         for each(_loc2_ in this.daysClaimed)
         {
            param1.writeShort(65535 & ((65535 & _loc2_) >>> 15 | (65535 & _loc2_) << 1));
         }
         param1.writeBoolean(this.isClaimable);
      }
   }
}

