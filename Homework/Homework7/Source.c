#include "Header.h"



int hashCode(struct hashTable* m_hashTable, int m_key) {
	return m_key % m_hashTable->maxSize;
}


struct data* search(struct hashTable* m_hashTable, int m_key) {
	printf("Searching for an item with key %d.\n", m_key);

	if (m_hashTable) {
		//get the hash 
		int hashIndex = hashCode(m_hashTable, m_key);

		//move in array until an empty 
		while (m_hashTable->hashArray[hashIndex].key != UNDEFINED_DATA) {

			if (m_hashTable->hashArray[hashIndex].key == m_key)
				return &m_hashTable->hashArray[hashIndex];


			//go to next cell
			++hashIndex;


			//wrap around the table
			hashIndex %= m_hashTable->maxSize;
		}
		return NULL;
	}
	else {
		return NULL;
	}
}

void insert(struct hashTable* m_hashTable, int m_key, int m_data) {
	printf("Inserting an item (%d, %d).\n", m_key, m_data);

	if (m_hashTable) {
		struct data* item = (struct data*) malloc(sizeof(struct data));
		if (item) {
			item->data = m_data;
			item->key = m_key;
		}
		else {
			printf("Cannot create new memory for new item.\n");
		}


		// Get the hash 
		int hashIndex = hashCode(m_hashTable, m_key);


		// Move in array until meet an empty or deleted cell
		while (m_hashTable->hashArray[hashIndex].key != UNDEFINED_DATA) {
			//go to next cell
			++hashIndex;


			//wrap around the table
			hashIndex %= m_hashTable->maxSize;
		}


		m_hashTable->hashArray[hashIndex] = *item;
	}
	else {
		return;
	}
}

struct data* deleteData(struct hashTable* m_hashTable, struct data* item) {
	printf("Deleting item: (%d, %d).\n", item->key, item->data);

	if (m_hashTable) {
		int key = item->key;

		//get the hash 
		int hashIndex = hashCode(m_hashTable, key);

		//move in array until an empty
		while (m_hashTable->hashArray[hashIndex].key != UNDEFINED_DATA) {

			if (m_hashTable->hashArray[hashIndex].key == key) {
				struct data* temp = &m_hashTable->hashArray[hashIndex];

				m_hashTable->hashArray[hashIndex].key = UNDEFINED_DATA;
				return temp;
			}

			//go to next cell
			++hashIndex;

			//wrap around the table
			hashIndex %= (m_hashTable->maxSize);
		}
	}
	else {
		return NULL;
	}
}

void show(struct hashTable* m_hashTable) {
	if (m_hashTable) {
		int i = 0;

		for (i = 0; i < m_hashTable->maxSize; i++) {

			if (m_hashTable->hashArray[i].key != UNDEFINED_DATA)
				printf(" (%d,%d)", m_hashTable->hashArray[i].key, m_hashTable->hashArray[i].data);
			else
				printf(" ~~ ");
		}

		printf("\n");
	}
	else {
		return;
	}
}


struct hashTable* initHashTable(int m_maxSize) {
	printf("Creating a new hash table with max size %d.\n", m_maxSize);

	struct hashTable* newHashTable = (struct hashTable*)malloc(sizeof(struct hashTable));


	if (newHashTable) {
		newHashTable->maxSize = m_maxSize;
		newHashTable->hashArray = (struct data*)malloc(sizeof(struct data) * newHashTable->maxSize);

		if (newHashTable->hashArray) {
			for (int i = 0; i < newHashTable->maxSize; i++) {
				newHashTable->hashArray[i].key = UNDEFINED_DATA;
			}
			return newHashTable;
		}
		else {
			printf("Cannot create new memory.");
			free(newHashTable);
			return NULL;
		}
	}
	else {
		printf("Cannot create new memory.");
		return NULL;
	}
}


void freeHashTable(struct hashTable* m_hashTable) {
	if (m_hashTable) {
		if (m_hashTable->hashArray) {
			free(m_hashTable->hashArray);
		}
		else {
			//
		}
		free(m_hashTable);
	}
	else {
		return;
	}
}

