namespace ChatServer.Hubs
{
    public class UserMessageDto
    {
        public UserInfoDto User { get; set; }
        public string Content { get; set; }
        public string Time { get; set; }
    }
}
