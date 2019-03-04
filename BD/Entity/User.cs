using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BD.Entity
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string Email { get; set; }
        public DateTime DateOfBirthday { get; set; }
        [Required]
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
    }
    //class Chat
    //{
    //    public int Id { get; set; }
    //    public int FirstUserId { get; set; }
    //    [ForeignKey("FirstUserId")]
    //    public User User { get; set; }
    //    public int SecondUserId { get; set; }
    //    [ForeignKey("SecondUserId")]
    //    public User User { get; set; }
    //}
    //class Massage
    //{
    //    public int Id { get; set; }
    //    public int ChatId { get; set; }
    //    public int SenderId { get; set; }
    //    public DateTime DateTime { get; set; }
    //    public string Text { get; set; }
    //}
}
