using DarksideApi.Util;
using DarksideApi.Util.Collections;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Numerics;

namespace DarksideApi.DarkOrbit.Objects.Group
{
    public class GroupObj : IGameObject, INotifyPropertyChanged
    {
        private readonly Api api;

        public event PropertyChangedEventHandler? PropertyChanged;

        public string LeaderUsername { get; set; }
        public int LeaderUserId { get; set; }
        public ObservableKeyedCollection<int, GroupMemberObj> Members { get; private set; } = new ObservableKeyedCollection<int, GroupMemberObj>(g => g.Id, "Id");
        public ConcurrentDictionary<string, DateTime> Invited { get; set; }

        public int X { get; set; }
        public int Y { get; set; }
        public Vector2 Position => new(this.X, this.Y);

        public double CurrentDistance => double.MaxValue;
        public double TargetDistance => double.MaxValue;
        public double PathDistance => double.MaxValue;
        public bool HasApi => this.api != null;

        public int MemberCount => this.Members.Count;   

        public GroupObj(Api api, string leader, int userId)
        {
            this.api = api;
            this.LeaderUsername = leader;
            this.LeaderUserId = userId;
            //this.Members = new ObservableKeyedCollection<int, GroupMemberObj>(g => g.Id, "Id");
            this.Invited = [];
        }

        public void AddMember(GroupMemberObj groupMember)
        {
            /*if (this.Members.TryAdd(groupMember.Player.UserId, groupMember))
            {
                return;
            }
            this.Members[groupMember.Player.UserId] = groupMember;*/
            this.api.AddGroupMembers([groupMember]);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(this.Members)));
        }

        public bool IsInvited(string username)
        {
            if (this.Members.ToList().Exists(s => s.Player.Username == username))
            {
                return true;
            }
            if (this.Invited.TryGetValue(username, out DateTime inviteTime))
            {
                if ((DateTime.UtcNow - inviteTime).TotalMinutes > 5)
                {
                    this.Invited.Remove(username, out _);
                    return false;
                }
                return true;
            }
            return false;
        }

        public void Invite(string username)
        {
            if (!HelpTools.Trim(username, out var toInvite) || this.IsInvited(username))
            {
                return;
            }
            this.api.GameMethods.GroupInvite(toInvite);
            this.Invited.TryAdd(username, DateTime.UtcNow);
            this.api.WriteLog($"Inviting {toInvite} to Group...");
        }

        public void AcceptInvite()
        {
            this.api.GameMethods.GroupAccept(this.LeaderUserId);
            this.Invited.Remove(this.LeaderUsername, out _);
        }

        public bool IsInGroup(int userId)
        {
            return this.Members.Any(m => m.Player.UserId == userId);
        }

        public bool IsInGroup(string username)
        {
            return this.Members.Any(m => m.Player.Username.Equals(username, StringComparison.OrdinalIgnoreCase));
        }
    }
}
