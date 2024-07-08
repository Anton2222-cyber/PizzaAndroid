import React, { useEffect, useState } from 'react';
import { Image, StyleSheet, TouchableOpacity, View, Dimensions } from 'react-native';
import axios from "axios";
import ParallaxScrollView from '@/components/ParallaxScrollView';
import { ThemedText } from '@/components/ThemedText';
import { ThemedView } from '@/components/ThemedView';
import { SERVER_URL } from '@env';

// Оголосіть інтерфейс Category
interface Category {
    id: number;
    name: string;
    image: string;
}

const { width } = Dimensions.get('window');

export default function HomeScreen() {
    const [categories, setCategories] = useState<Category[]>([]);

    const server = SERVER_URL;

    useEffect(() => {
        axios.get<Category[]>(`${server}/api/categories/GetAll`)
            .then(resp => {
                console.log('Response server', resp.data);
                setCategories(resp.data);
            })
            .catch(error => {
                console.log('Error server', error);
            });
    }, []);

    const handleCategoryPress = (category: Category) => {
        console.log('Pressed category:', category.name);
    };

    return (
        <ParallaxScrollView
            headerBackgroundColor={{ light: '#ffffff', dark: '#1D3D47' }}
            headerImage={
                <Image
                    source={require('@/assets/images/pizza-image.jpg')}
                    style={styles.reactLogo}
                />
            }>
            <ThemedView style={styles.titleContainer}>
                <ThemedText type="title">Категорії</ThemedText>
            </ThemedView>
            {categories.map(category => (
                <TouchableOpacity
                    key={category.id}
                    style={styles.categoryCard}
                    onPress={() => handleCategoryPress(category)}>
                    <Image
                        source={{ uri: `${server}/images/200_${category.image}` }}
                        style={styles.categoryImage}
                    />
                    <View style={styles.categoryInfo}>
                        <ThemedText style={styles.categoryName}>{category.name}</ThemedText>
                    </View>
                </TouchableOpacity>
            ))}
        </ParallaxScrollView>
    );
}

const styles = StyleSheet.create({
    titleContainer: {
        flexDirection: 'row',
        alignItems: 'center',
        justifyContent: 'center',
        marginVertical: 16,
    },
    categoryCard: {
        flexDirection: 'row',
        alignItems: 'center',
        backgroundColor: '#ffffff',
        borderRadius: 10,
        marginHorizontal: 16,
        marginBottom: 16,
        shadowColor: '#000',
        shadowOffset: { width: 0, height: 2 },
        shadowOpacity: 0.25,
        shadowRadius: 3.84,
        elevation: 5,
        width: width - 32,
    },
    categoryImage: {
        width: 150,
        height: 150,
        borderTopLeftRadius: 10,
        borderBottomLeftRadius: 10,
    },
    categoryInfo: {
        flex: 1,
        padding: 16,
    },
    categoryName: {
        fontSize: 20,
        fontWeight: 'bold',
    },
    reactLogo: {
        height: 178,
        width: "100%",
        bottom: 0,
        left: 0,
        position: 'absolute',
    },
});
