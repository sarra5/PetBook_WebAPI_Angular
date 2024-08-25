using Microsoft.EntityFrameworkCore;
using PetBooK.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PetBooK.BL.Reo
{
    public class GenericRepo<TEntity> where TEntity : class
    {
        PetBookContext db;

        public GenericRepo(PetBookContext db)
        {
            this.db = db;
        }

        public List<TEntity> selectall()
        {
            return db.Set<TEntity>().ToList();
        }

        // EX: Reservation has a relation with clinic and pet, to get the pet data and the clinic data we need include not just select
        public List<TEntity> SelectAll(params Expression<Func<TEntity, object>>[] includes)
        {
            IQueryable<TEntity> query = db.Set<TEntity>();

            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            return query.ToList();
        }

        public TEntity selectbyid(params object[] keyValues)
        {
            return db.Set<TEntity>().Find(keyValues);
        }

        public void add(TEntity entity)
        {
            db.Set<TEntity>().Add(entity);

        }

        public void update(TEntity entity)
        {
            db.Entry(entity).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
        }

        public void delete(int id)
        {
            TEntity obj = db.Set<TEntity>().Find(id);
            db.Set<TEntity>().Remove(obj);
        }

        public void deleteEntity(TEntity entity)
        {
            db.Set<TEntity>().Remove(entity);
        }


        public int Count()
        {
            return db.Set<TEntity>().Count();
        }


        // This method allows you to find entities based on a predicate.
        // example  :Expression<Func<Product, bool>> predicate = p => p.Price > 100;    var expensiveProducts= productRepo.FindBy(predicate);

        public List<TEntity> FindBy(Expression<Func<TEntity, bool>> predicate)
        {
            return db.Set<TEntity>().Where(predicate).ToList();
        }


        //This method returns the first entity that matches a given predicate, or null if no such entity is found.
        public TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate)
        {
            return db.Set<TEntity>().FirstOrDefault(predicate);
        }

        public List<TEntity> SelectAllWithIncludes(params Expression<Func<TEntity, object>>[] includes)
        {
            IQueryable<TEntity> query = db.Set<TEntity>();

            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            return query.ToList();
        }
        public Pet selectbyPetid(int id)
        {
            return db.Set<Pet>()
                     .Include(p => p.Pet_Breeds)
                     .ThenInclude(pb => pb.Breed)
                     .FirstOrDefault(p => p.PetID == id);
        }


        public List<TEntity> FindByInclude(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includes)
        {
            IQueryable<TEntity> query = db.Set<TEntity>();

            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            return query.Where(predicate).ToList();
        }
        public List<TEntity> FindByIncludeThenInclude<TProperty, TThenProperty>(Expression<Func<TEntity, bool>> predicate,Expression<Func<TEntity, IEnumerable<TProperty>>> includes, Expression<Func<TProperty, TThenProperty>> thenIncludes)
        {
            IQueryable<TEntity> query = db.Set<TEntity>();

            query = query.Include(includes).ThenInclude(thenIncludes);

            return query.Where(predicate).ToList();
        }


        public void DeleteEntities(List<TEntity> entities)
        {
            db.Set<TEntity>().RemoveRange(entities);
        }
        public TEntity SelectByIDInclude(int id, string IdName, params Expression<Func<TEntity, object>>[] includes)
        {
            IQueryable<TEntity> query = db.Set<TEntity>();

            foreach (var include in includes)
            {
                query = query.Include(include);
            }
            return query.FirstOrDefault(e => EF.Property<int>(e, IdName) == id);
        }

        public TEntity SelectByCompositeKeyInclude(string FID, int id1, string SID, int id2, params Expression<Func<TEntity, object>>[] includes)
        {
            IQueryable<TEntity> query = db.Set<TEntity>();

            foreach (var include in includes)
            {
                query = query.Include(include);
            }
            return query.FirstOrDefault(e => EF.Property<int>(e, FID) == id1 && EF.Property<int>(e, SID) == id2);
        }


        public List<TEntity> FindByAndSetForeignKeyToNull(
         Expression<Func<TEntity, bool>> predicate,
            Expression<Func<TEntity, object>> foreignKeySelector)
        {

            var entities = db.Set<TEntity>().Where(predicate).ToList();
            var foreignKeyProperty = (foreignKeySelector.Body as MemberExpression ??
                                      ((UnaryExpression)foreignKeySelector.Body).Operand as MemberExpression).Member as PropertyInfo;



            foreach (var entity in entities)
            {
                foreignKeyProperty.SetValue(entity, null);
            }


            return entities;
        }

        public TEntity SelectBy3CompositeKeyInclude(string FID, int id1, string SID, int id2, string THID, int id3, params Expression<Func<TEntity, object>>[] includes)
        {
            IQueryable<TEntity> query = db.Set<TEntity>();

            foreach (var include in includes)
            {
                query = query.Include(include);
            }
            return query.FirstOrDefault(e => EF.Property<int>(e, FID) == id1 && EF.Property<int>(e, SID) == id2 && EF.Property<int>(e, THID) == id3);
        }
        public List<TEntity> GetEntitiesByUserId(int userId, string userIdPropertyName)
        {
            var parameter = Expression.Parameter(typeof(TEntity), "e");
            var property = Expression.Property(parameter, userIdPropertyName);
            var constant = Expression.Constant(userId);
            var equality = Expression.Equal(property, constant);
            var predicate = Expression.Lambda<Func<TEntity, bool>>(equality, parameter);

            return db.Set<TEntity>().Where(predicate).ToList();
        }
        public string GetBreedTypeByPetId(int petId)
        {
            var breedType = (from pet in db.Pets
                             join petBreed in db.Pet_Breeds on pet.PetID equals petBreed.PetID
                             join breed in db.Breeds on petBreed.BreedID equals breed.BreedID
                             where pet.PetID == petId
                             select breed.Breed1).FirstOrDefault();

            return breedType;
        }
        public bool PairPets(int petId, int selectedPetId, int userId)
        {
            var currentPet = db.Pets.Include(p => p.Pet_Breeds).ThenInclude(pb => pb.Breed)
                                    .FirstOrDefault(p => p.PetID == petId);
            if (currentPet == null || !currentPet.ReadyForBreeding)
            {
                return false;
            }

            var userPets = db.Pets.Include(p => p.Pet_Breeds).ThenInclude(pb => pb.Breed)
                                  .Where(p => p.UserID == userId).ToList();

            var matchingPet = userPets.FirstOrDefault(p => p.PetID == selectedPetId);
            if (matchingPet == null)
            {
                return false;
            }

           
            if (currentPet.Sex == matchingPet.Sex) //  one pet is female and the other is male
            {
                return false;
            }

            
            if (currentPet.Type != matchingPet.Type)//both pets are of the same type 
            {
                return false;
            }

            // Check breed compatibility based on the type of pet
            bool breedsMatch;
            if (currentPet.Type == "Dog")
            {
               // ensure the breeds are the same
                breedsMatch = matchingPet.Pet_Breeds.Any(pb => currentPet.Pet_Breeds.Any(cpb => cpb.BreedID == pb.BreedID));
            }
            else if (currentPet.Type == "Cat")
            {
                // For cats, allow any breed
                breedsMatch = true;
            }
            else
            {
                // If pet type is neither Dog nor Cat, return false
                return false;
            }

            if (breedsMatch)
            {
                var requestForBreed = new Request_For_Breed
                {
                    PetIDSender = matchingPet.PetID,
                    PetIDReceiver = petId,
                    Pair = false
                };

                db.Request_For_Breeds.Add(requestForBreed);
                db.SaveChanges();
                return true;
            }

            return false;
        }



        public IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> predicate)
        {
            return db.Set<TEntity>().Where(predicate);
        }


        public List<TEntity> FindByForeignKey(Expression<Func<TEntity, bool>> predicate)
        {

            return db.Set<TEntity>().Where(predicate).ToList();

        }

        public List<Pet> GetPetsByUser(int userId)
        {
            return  db.Pets
                .Where(p => p.UserID == userId)
                .ToList();
        }



        public List<TEntity> FindByForeignKeyInclude(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includes)
        {
            IQueryable<TEntity> query = db.Set<TEntity>().Where(predicate);

            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            return query.ToList();

        }
        public List<TEntity> SelectAllIncludePagination(params Expression<Func<TEntity, object>>[] includes)
        {
            IQueryable<TEntity> query = db.Set<TEntity>();

            foreach (var include in includes)
            {
                query = query.Include(include);
            }


            return query.ToList();
        }
        public List<TEntity> FindByIdInclude(int Id, string str,params Expression<Func<TEntity, object>>[] includes)
        {
            IQueryable<TEntity> query = db.Set<TEntity>();

            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            return query.Where(e => EF.Property<int>(e, str) == Id).ToList();
        }




    }
}