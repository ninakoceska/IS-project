using BookStore.Domain.Domain;
using BookStore.Repository.Interface;
using BookStore.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Service.Implementation
{
    public class PublisherService : IPublisherService
    {
        private readonly IRepository<Publisher> _publisherRepository;

        public PublisherService(IRepository<Publisher> publisherRepository)
        {
            _publisherRepository = publisherRepository;
        }

        public List<Publisher> GetAllPublishers()
        {
            return _publisherRepository.GetAll().ToList();
        }

        public Publisher GetPublisherDetails(Guid id)
        {
            return _publisherRepository.Get(id);
        }

        public void CreatePublisher(Publisher publisher)
        {
            _publisherRepository.Insert(publisher);
        }

        public void UpdatePublisher(Publisher publisher)
        {
            _publisherRepository.Update(publisher);
        }

        public void DeletePublisher(Guid id)
        {
            var publisher = _publisherRepository.Get(id);
            _publisherRepository.Delete(publisher);
        }
    }
}
