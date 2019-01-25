using System;
using System.IO;
using System.Collections.Generic;
using SecretSanta.Domain.Models;

namespace SecretSanta.Import.Services
{
    public class GiftImportService
    {
        public static Wishlist ReadWishlist(string filename)
        {
            if (string.IsNullOrEmpty(filename))
                throw new ArgumentNullException("Filename cannot be null or empty.");

            string path = Path.Combine(System.IO.Path.GetTempPath() + @"\", filename);

            if (!File.Exists(filename))
            {
                if (!File.Exists(path))
                {
                    throw new FileNotFoundException("Could not find file " + filename + " at path.");
                }
            }
            else
                path = filename;

            List<Gift> gifts = new List<Gift>();
            User user = new User();

            using (StreamReader sr = File.OpenText(path))
            {
                string line;
                int order = 1;

                line = sr.ReadLine();

                user = GetUserFromHeader(line);

                while((line = sr.ReadLine()) != null)
                {
                    if(!string.IsNullOrEmpty(line) && !string.IsNullOrWhiteSpace(line))
                    {
                        gifts.Add(new Gift
                        {
                            Title = line,
                            OrderOfImportance = order
                        });

                        order++;
                    }
                }
            }

            Wishlist wl = new Wishlist
            {
                Gifts = gifts,
                User = user
            };

            return wl;
        }

        public static User GetUserFromHeader(string header)
        {
            if (string.IsNullOrEmpty(header))
                return null;
            if (!header.StartsWith("Name: "))
                return null;

            string[] name;

            string[] headerSplit = header.Trim().Split(':');

            string nameTrimmed = headerSplit[1].Trim();

            if (header.Contains(","))
                name = nameTrimmed.Split(',');
            else
                name = nameTrimmed.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);

            if (name.Length != 2)
                return null;

            return new User {
                FirstName = name[0].Trim(),
                LastName = name[1].Trim()
            };
        }
    }
}
